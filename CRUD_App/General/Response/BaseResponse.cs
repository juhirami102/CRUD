using CRUD_App.General.Resources.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_App.General.Response
{
    public class BaseResponse
    {
        private readonly IResourceService _resourceService;
        //public BaseResponse(IResourceService resourceService)
        //{
        //    _resourceService = resourceService;
        //}
        public string GetSqlExceptionMessage(int number)
        {
            //set default value which is the generic exception message
            string error = string.Empty;// MyConfiguration.Texts.GetString(ExceptionKeys.DalExceptionOccured);
            switch (number)
            {
                case 4060:
                    // Invalid Database
                    error = _resourceService.GetString("InvalidDatabase");
                    break;
                case 18456:
                    // Login Failed
                    error = _resourceService.GetString("LoginFailed");
                    break;
                case 547:
                    // ForeignKey Violation
                    error = _resourceService.GetString("ForeignKeyViolation");
                    break;
                case 2627:
                    // Unique Index/Constriant Violation
                    error = _resourceService.GetString("ConstriantViolation");
                    break;
                case 2601:
                    // Unique Index/Constriant Violation
                    error = _resourceService.GetString("UniqueIndexViolation");
                    break;
                default:
                    // throw a general DAL Exception
                    error = _resourceService.GetString("DALException");
                    break;
            }

            return error;
        }
    }
}
