using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CRUD_App.General.Resources.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CRUD_App.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        #region properties
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        #endregion
        #region Constructor
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<Startup> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }
        #endregion
        #region Methods
        /// <summary>
        /// purpose : To Handle exception and write in log file
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(context, exception);
            }
        }
        private Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            Boolean IsfileNewCreated = false;      // flag for check new or existing file.
            Assembly asm = Assembly.GetExecutingAssembly();

            string solutiondir = Path.GetDirectoryName(asm.Location);      //get path of solution
            string FolderPath = Path.Combine(solutiondir, _configuration["LogfileDir"]);      // combine path with folder name
            string FilePath = Path.Combine(FolderPath, _configuration["FileName"]+".txt" ); // combine path with file name

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);// create folder if not exist

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Dispose(); // create text file if not exist
                IsfileNewCreated = true;
            }

            var files = new DirectoryInfo(FolderPath).GetFiles("*.*"); // get all files from folders.
            string LatestFile = "";
            DateTime Lastupdated = DateTime.MinValue;
            foreach(FileInfo file in files)
            {
                if(file.LastWriteTime > Lastupdated)
                {
                    Lastupdated = file.LastWriteTime;
                    LatestFile = file.Name; // get latest file name for further 2 mb size compare.
                }
            }

            string FilePathForSize = Path.Combine(FolderPath, LatestFile); // cobine folder path with latest file name.
            if (!string.IsNullOrEmpty(LatestFile))
            {
                FilePath = FilePathForSize; // assign latest file into global variable for further use.
                long mb = Helper.GetFileSizeInMB(FilePath); // get files size in mb.
                if (File.Exists(FilePathForSize) && mb >= 2) // check already exist files having size more then 2 mb
                {
                    FilePath = FolderPath + "\\" + _configuration["FileName"] + DateTime.Now.ToString("dd-MM-yyyy").Replace("-", "_") + ".txt"; // combine path for new file name concate with current date
                    File.Create(FilePath).Dispose(); // create new text file when existing file exceed 2mb 
                    IsfileNewCreated = true;
                }
            }
          
            string ExceptionInfo = "Date Time- "+ DateTime.Now.ToString();
            ExceptionInfo += "\n" + "Message- " + exception.Message;
            ExceptionInfo += "\n" + "Inner Exception - " + exception.InnerException;
            ExceptionInfo += "\n" + "Strack Trace - " + exception.StackTrace;
            ExceptionInfo += "\n" + "___________________________________________________________________________________";

            if (IsfileNewCreated)
            File.WriteAllText(FilePath, ExceptionInfo); // write text in new file
            else
                File.AppendAllText(FilePath, "\n"+ ExceptionInfo); // append text in existing file.

            var response = new { message = exception.Message };
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;

            return context.Response.WriteAsync(payload);
        }
        #endregion
    }

}
