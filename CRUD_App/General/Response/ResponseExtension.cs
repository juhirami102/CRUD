using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Reflection;

namespace CRUD_App.General.Response
{
    public static class ResponseExtension
    {
        /// <summary>
        /// Implemetion of Generic Extension Method for getting HttpResponse
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        public static IActionResult ToHttpResponse<T>(this T response)
        {
            var status = HttpStatusCode.OK;
            if (response != null)
            {
                var _success = (bool)response.GetType().GetProperty("Success").GetValue(response);
                if (response.GetType().GetProperty("Data").GetValue(response) == null)
                {
                    //Change No Content to Not Founds
                    status = HttpStatusCode.NotFound;
                }
                else if (!_success)
                {
                    status = HttpStatusCode.InternalServerError;
                }
            }
            return new ObjectResult(response)
            {
                StatusCode = (Int32)status
            };
        }
        /// <summary>
        /// Implemetion of Generic Extension Method for getting HttpResponseForConflict
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        public static IActionResult ToHttpResponseConflict<T>(this T response)
        {
            var status = HttpStatusCode.Conflict;//409
            return new ObjectResult(response)
            {
                StatusCode = (Int32)status
            };
        }
        /// <summary>
        /// Implemetion of Generic Extension Method for Return Status Code
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        public static IActionResult ToHttpResponse<T>(this T response, int StatusCode)
        {
            try { response.GetType().GetProperty("Success").SetValue(response, false); } catch { }
            var status = StatusCode;
            return new ObjectResult(response)
            {
                StatusCode = (Int32)status
            };
        }
    }
}
