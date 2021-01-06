using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_App.General.Response
{
    /// <summary>
    /// Generic Interface to Handle API Responses
    /// </summary>
    public interface IResponse
    {
        String Message { get; set; }
        Boolean Success { get; set; }
        string NotificationType { get; set; }
        string ReturnToUrl { get; set; }
        List<ErrorMessage> ErrorMessages { get; set; }
    }
}
