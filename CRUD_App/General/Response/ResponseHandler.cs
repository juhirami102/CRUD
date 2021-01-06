using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using CRUD_App.General.Logging;
using System.Reflection;
using CRUD_App.General.Resources.Services;
using CRUD_App.General.Resources.Common;

namespace CRUD_App.General.Response
{
    /// <summary>
    /// Implemetion of Generic Base Response
    /// TModel will be Entity and T will be Dervived class here
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ResponseHandler<TModel, T> : BaseResponse, IErrorHandler<T> where T : class
    {
        public T _object { get; set; }
        public String Message { get; set; }
        public Boolean Success { get; set; }
        public string ReturnToUrl { get; set; }
        public string NotificationType { get; set; }
        public List<ValidationError> ValidationMessages { get; set; }

        private List<ErrorMessage> _errorMessages = new List<ErrorMessage>();

        public List<ErrorMessage> ErrorMessages
        {
            get
            {
                return _errorMessages ?? (_errorMessages = new List<ErrorMessage>());
            }
            set
            {
                _errorMessages = value;
            }
        }
        public ResponseHandler<TModel, T> Failed()
        {
            this.Success = false;
            this.Message =Helper.ErrorCodeRecevied;
            return this;
        }
        public ResponseHandler<TModel, T> Failed(Exception ex)
        {
            this.Success = false;
            this.Message = Helper.ErrorCodeRecevied;
            return this;
        }
        /// <summary>
        /// Passign the T reference with set all required properties in T _object
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public T WithError(Exception ex)
        {

            _object = (T)Activator.CreateInstance(typeof(T));
            if (ex is SqlException)
            {
                if (ex.InnerException != null)
                    ErrorMessages.Add(
                        new ErrorMessage(((SqlException)ex.InnerException).Number.ToString(),
                        GetSqlExceptionMessage(((SqlException)ex.InnerException).Number)));
                else
                {
                    ErrorMessages.Add(new ErrorMessage("SqlException", ex.Message));
                }
            }
            else
            {
                if (ex.InnerException != null)
                    ErrorMessages.Add(new ErrorMessage("", ex.Message + "\n" + ex.InnerException != null ? ex.InnerException.Message : ""));
                else
                    ErrorMessages.Add(new ErrorMessage("", ex.Message));
            }
            Failed(ex);

            _object.GetType().GetProperty("ErrorMessages", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, ErrorMessages);

            _object.GetType().GetProperty("Message", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, Message);

            _object.GetType().GetProperty("Success", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, false);

            return _object;
        }

        public T WithError(string errorMessage)
        {
            _object = (T)Activator.CreateInstance(typeof(T));
            Failed();
            ErrorMessages.Add(new ErrorMessage("", errorMessage));
            _object.GetType().GetProperty("ErrorMessages", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, ErrorMessages);
            return _object;
        }

        public T WithError(ModelStateDictionary modelState)
        {
            _object = (T)Activator.CreateInstance(typeof(T));
            this.Success = false;
            this.Message = Helper.ValidationFailed;
            ValidationMessages = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();

            _object.GetType().GetProperty("ErrorMessages", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, ErrorMessages);

            _object.GetType().GetProperty("Message", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, Message);

            _object.GetType().GetProperty("Success", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, false);

            _object.GetType().GetProperty("ValidationMessages", BindingFlags.Public | BindingFlags.Instance).SetValue(_object, ValidationMessages);

            return _object;
        }
    }
    /// <summary>
    /// Implemetion of Object data to handle API responses
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ObjectResponseHandler<TModel> : ResponseHandler<TModel, ObjectResponseHandler<TModel>>, IObjectResponseHandler<TModel>
    {
        public TModel Data { get; set; }
        public ObjectResponseHandler(bool success)
        {
            this.Success = success;
        }
        public ObjectResponseHandler()
        {
            this.Success = true;
        }
        public ObjectResponseHandler<TModel> Create()
        {
            return new ObjectResponseHandler<TModel>(true);
        }
        public ObjectResponseHandler<TModel> Create(TModel Data, string message)
        {
            ObjectResponseHandler<TModel> retResult = new ObjectResponseHandler<TModel>();
            retResult.Data = Data;
            retResult.Message = message;
            if (Data == null) retResult.Message = Helper.NoRecordFound;
            return retResult;
        }
        public ObjectResponseHandler<TModel> Create(TModel Data)
        {
            ObjectResponseHandler<TModel> retResult = new ObjectResponseHandler<TModel>();
            retResult.Data = Data;
            retResult.Message = Helper.OperationSuccess;
            if (Data == null) retResult.Message = Helper.NoRecordFound;
            return retResult;
        }
    }
    /// <summary>
    /// Implemetion of Datatable List to handle API responses
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class DataTableResponseHandler<TModel> : ResponseHandler<TModel, DataTableResponseHandler<TModel>>, IDataTableResponseHandler<TModel>
    {
        public DataTableResponseHandler(bool success)
        {
            this.Success = success;
        }
        public DataTableResponseHandler()
        {
            this.Success = true;
        }
        public Int32 PageSize { get; set; }
        public Int32 PageNumber { get; set; }
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<TModel> Data { get; set; }
        public DataTableResponseHandler<TModel> Create(int draw, int recordsTotal, int recordsFiltered, IEnumerable<TModel> Data)
        {
            DataTableResponseHandler<TModel> retResult = new DataTableResponseHandler<TModel>();
            retResult.draw = draw;
            retResult.recordsTotal = recordsTotal;
            retResult.recordsFiltered = recordsFiltered;
            retResult.Data = Data;
            return retResult;
        }
        public DataTableResponseHandler<TModel> Create(int draw, int recordsTotal, int recordsFiltered, IEnumerable<TModel> Data, string message)
        {
            DataTableResponseHandler<TModel> retResult = new DataTableResponseHandler<TModel>();
            retResult.draw = draw;
            retResult.recordsTotal = recordsTotal;
            retResult.recordsFiltered = recordsFiltered;
            retResult.Data = Data;
            retResult.Message = message;
            return retResult;
        }
    }
    /// <summary>
    /// Implemetion of IEnumerable List to handle API responses
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ListResponseHandler<TModel> : ResponseHandler<TModel, ListResponseHandler<TModel>>, IListResponseHandler<TModel>
    {
        public ListResponseHandler(bool success)
        {
            this.Success = success;
        }
        public ListResponseHandler()
        {
            this.Success = true;
        }
        public IEnumerable<TModel> Data { get; set; }
        public ListResponseHandler<TModel> Create(IEnumerable<TModel> Data)
        {
            ListResponseHandler<TModel> retResult = new ListResponseHandler<TModel>();
            retResult.Data = Data;
            return retResult;
        }
        public ListResponseHandler<TModel> Create(IEnumerable<TModel> Data,
            string message)
        {
            ListResponseHandler<TModel> retResult = new ListResponseHandler<TModel>();
            retResult.Data = Data;
            retResult.Message = message;
            return retResult;
        }
    }
}
