using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Go2Share.General
{
    public class FileUploadHelper
    {
        public async static Task<string> SaveSingleFile(IConfiguration _configuration, string RootPath, string folderName, IFormFile singleFile)
        {
            if (singleFile.Length > 0)
            {
                var WWRootPath = RootPath;
                var DirPath = Path.Combine(WWRootPath, _configuration["FileUpload:FileDir"]);
                //var SubDir = Path.Combine(DirPath, _configuration["ImageSubDir"]);
                var childDir = Path.Combine(DirPath, folderName);
                if (!Directory.Exists(DirPath))
                    Directory.CreateDirectory(DirPath);// create dir if not exist

                //if (!Directory.Exists(SubDir))
                //    Directory.CreateDirectory(SubDir);// create sub dir if not exist

                if (!Directory.Exists(childDir))
                    Directory.CreateDirectory(childDir);// create sub dir if not exist

                var imgname = DateTime.Now.Ticks + "_" + singleFile.FileName;

                if (singleFile.Length > 0)
                {
                    //var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgname);
                    using (var fileStream = new FileStream(Path.Combine(childDir, imgname), FileMode.Create))
                    {
                        await singleFile.CopyToAsync(fileStream);
                    }

                }
                return imgname;
            }
            return "";
        }
        public async static Task<List<string>> SaveMultipleFile(IConfiguration _configuration, string RootPath, string folderName, List<IFormFile> files)
        {
            List<string> fileNames = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                var imgName = await SaveSingleFile(_configuration, RootPath, folderName, files[i]);
                if (!string.IsNullOrEmpty(imgName))
                    fileNames.Add(imgName);
            }
            return fileNames;
        }
        public static void DeleteMultipleFile(IConfiguration _configuration, string RootPath, string folderName, List<string> files)
        {
            List<string> fileNames = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                DeleteFile(_configuration, RootPath, folderName, files[i]);
            }
        }
        public static void DeleteFile(IConfiguration _configuration, string RootPath, string folderName, string oldFileName)
        {
            var WWRootPath = RootPath;
            var DirPath = Path.Combine(WWRootPath, _configuration["FileUpload:FileDir"]);
            //var SubDir = Path.Combine(DirPath, _configuration["ImageSubDir"]);
            var childDir = Path.Combine(DirPath, folderName);
            var filePath = Path.Combine(childDir, oldFileName);
            if (File.Exists(filePath))
            {
                System.IO.File.Delete(filePath); // delete image from dir
            }
        }

    }
}
