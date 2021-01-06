using CRUD_App.API.Middlewares.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace CRUD_App.API.Middlewares
{
    public class TokenManagerMiddleware : IMiddleware
    {
        #region Properties
        private readonly ITokenManager _tokenManager;
        #endregion
        #region Constructor
        public TokenManagerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }
        #endregion
        #region Methods
        /// <summary>
        /// purpose : To Handle request with Token; if token active 
        /// then will invoke next request else return unathorized
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //var myContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            //var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.).ControllerName;
            if (context.Request.Path.Value.Contains("/Auth/login"))
            {
                await next.Invoke(context); // call next middleware 
                return;
            }
            if (context.Request.Path.Value.Contains("/api/User/UserRegister"))
            {
                await next.Invoke(context); // call next middleware 
                return;
            }
            if (await _tokenManager.IsCurrentActiveToken())
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        #endregion
    }
}
