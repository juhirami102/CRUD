using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using CRUD_App.Web.Helper;
using CRUD_App.Entity.Entity;

namespace CRUD_App.API.Middlewares
{
    public class AuthMiddleware
    {
        #region properties
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        #endregion
        #region Constructor
        public AuthMiddleware(RequestDelegate next, ILogger<Startup> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }
        #endregion
        #region Methods
        /// <summary>
        /// purpose : To Handle the token and user logged in values
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(context.GetTokenAsync("token").Result))
                {
                    if (string.IsNullOrEmpty(context.Session.GetString(UserHelper.LoggedInUserEmailAddress)))
                    {
                        UserLoggedInModel oUserLoggedInModel = UserHelper.GetLoggedInUser(context.GetTokenAsync("token").Result);
                        context.Session.SetString(UserHelper.LoggedInUserEmailAddress, string.IsNullOrEmpty(oUserLoggedInModel.EmailAddress)?"": oUserLoggedInModel.EmailAddress);
                        context.Session.SetString(UserHelper.LoggedInUserName, string.IsNullOrEmpty(oUserLoggedInModel.FirstName + " " + Convert.ToString(oUserLoggedInModel.LastName))?"": oUserLoggedInModel.FirstName + " " + Convert.ToString(oUserLoggedInModel.LastName));
                        context.Session.SetInt32(UserHelper.LoggedInUserId, oUserLoggedInModel.UserId);
                    }
                }

                await _next(context);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion
    }

}
