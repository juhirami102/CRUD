using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CRUD_App.General.Response
{
    /// <summary>
    /// Interface for Generic Error Handling for Datatable , List and Single Object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IErrorHandler<T> where T : class
    {
        T WithError(Exception ex);
        T WithError(string errorMessage);
        T WithError(ModelStateDictionary modelState);
    }
    /// <summary>
    /// Interface to handle single object API response handler
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IObjectResponseHandler<TModel> : IResponse, IErrorHandler<ObjectResponseHandler<TModel>>
    {
        TModel Data { get; set; }
        ObjectResponseHandler<TModel> Create();
        ObjectResponseHandler<TModel> Create(TModel Data);
        ObjectResponseHandler<TModel> Create(TModel Data, string message);
    }
    /// <summary>
    /// Interface to handle Datatable API response handler
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IDataTableResponseHandler<TModel> : IResponse, IErrorHandler<DataTableResponseHandler<TModel>> //IResponseHandler<TModel>
    {
        int draw { get; set; }
        int recordsTotal { get; set; }
        int recordsFiltered { get; set; }
        new IEnumerable<TModel> Data { get; set; }
        Int32 PageSize { get; set; }
        Int32 PageNumber { get; set; }
        DataTableResponseHandler<TModel> Create(int draw, int recordsTotal, int recordsFiltered, IEnumerable<TModel> Data);
        DataTableResponseHandler<TModel> Create(int draw, int recordsTotal, int recordsFiltered, IEnumerable<TModel> Data, string message);
    }
    /// <summary>
    /// Interface to handle IEnurable List Data API response handler
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IListResponseHandler<TModel> : IResponse, IErrorHandler<ListResponseHandler<TModel>>
    {
        IEnumerable<TModel> Data { get; set; }
        ListResponseHandler<TModel> Create(IEnumerable<TModel> Data);
        ListResponseHandler<TModel> Create(IEnumerable<TModel> Data, string message);
    }
}
