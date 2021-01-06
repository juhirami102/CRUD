using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_App.General.Response
{
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }

    public class ErrorMessage
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; }

        public string Message { get; }

        public ErrorMessage(string code, string message)
        {
            Code = code != string.Empty ? code : null;
            Message = message;
        }
    }
}
