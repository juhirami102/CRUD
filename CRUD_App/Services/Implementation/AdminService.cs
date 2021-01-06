using CRUD_App.Core.Repositories;
using CRUD_App.Core.UnitOfWork;
using CRUD_App.Data.Entity;
using CRUD_App.Entity.Entity;
using CRUD_App.General;
using CRUD_App.General.Datatable;
using CRUD_App.General.Resources.Common;
using CRUD_App.General.Resources.Services;
using Go2Share.Services.Interfaces;
using Go2Share.Services.ModelMapper;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Go2Share.Services.Implementation
{
    public class AdminService : IAdminService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperFactory _mapperFactory;
        private readonly IResourceService _messageService;
        private IRepository<dynamic> _repository { get; set; }
        #endregion

        #region Constructor
        public AdminService(IUnitOfWork unitOfWork, IMapperFactory mapperFactory, IResourceService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapperFactory = mapperFactory;
            _repository = _unitOfWork.GetRepository<dynamic>();
            _messageService = messageService;
        }
        #endregion

        #region Method
        public async Task<dynamic> GetDataBySPAsync(string SPName, List<string> ParamName = null, List<object> ParamValue = null)
        {
            return await _repository.GetDataBySPAsync(SPName, ParamName, ParamValue);
        }

        /// <summary>
        /// Purpose : Get Admin Dashboard Data
        /// </summary>
        /// <param name="CustomerRoleID"></param>
        /// <param name="OwnerRoleID"></param>
        /// <returns></returns>
        public async Task<dynamic> GetAdminDashboard(int CustomerRoleID, int OwnerRoleID)
        {
            List<string> ParamName = new List<string>();
            List<object> ParamValue = new List<object>();

            ParamName.Add("@CustomerRoleID");
            ParamName.Add("@OwnerRoleID");

            ParamValue.Add(CustomerRoleID);
            ParamValue.Add(OwnerRoleID);

            return await _repository.GetDataBySPAsync("STP_GoShare_GetAdminDashboard", ParamName,ParamValue);
        }

        /// <summary>
        /// Purpose : Get List of Booking Cancellation Requests
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public async Task<dynamic> GetListOfCancellationRequest(string StartDate, string EndDate)
        {
            List<string> ParamName = new List<string>();
            List<object> ParamValue = new List<object>();

            ParamName.Add("@StartDate");
            ParamName.Add("@EndDate");

            ParamValue.Add(StartDate);
            ParamValue.Add(EndDate);
            
            return await _repository.GetDataBySPAsync("STP_GoShare_GetListOfCancellationRequest", ParamName,ParamValue);
        }

        /// <summary>
        /// Purpose : Get List of Booking Refund Requests
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public async Task<dynamic> GetListOfRefundRequest(string StartDate, string EndDate, string FilePath)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@StartDate", StartDate);
            parameters.Add("@EndDate", EndDate);
            parameters.Add("@FilePath", FilePath);

            return await _repository.GetDataSetBySP("STP_GoShare_GetListOfRefundRequest", parameters);
        }

        /// <summary>
        /// Purpose : Get paged,sorted and filtered records as per model passed for list of Earnings
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPageSoretedFilteredEarningsFromSP(DataTableAjaxPostModel model)
        {
            string ColumnName = string.Empty;
            string SortDir = string.Empty;
            if (model.order.Count > 0)
            {
                ColumnName = model.columns[model.order[0].column].name;
                SortDir = model.order[0].dir;
            }

            List<string> ParamName = new List<string>();
            ParamName.Add("@CurrentPageNumber");
            ParamName.Add("@PageSize");
            ParamName.Add("@SortColumn");
            ParamName.Add("@SortDirection");
            ParamName.Add("@SearchText");

            List<object> ParamValue = new List<object>();
            ParamValue.Add(Convert.ToString(model.start)); // cur page no
            ParamValue.Add(Convert.ToString(model.length));// length
            ParamValue.Add(Convert.ToString(ColumnName));
            ParamValue.Add(Convert.ToString(SortDir)); // DESC
            ParamValue.Add(Convert.ToString(model.search.value));
            return await _repository.GetDataBySPAsync("STP_GoShare_GetListOfEarnings", ParamName, ParamValue);
        }

        /// <summary>
        /// Purpose : Get paged,sorted and filtered records as per model passed for list of Payments
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPageSoretedFilteredPaymentsFromSP(DataTableAjaxPostModel model)
        {
            string ColumnName = string.Empty;
            string SortDir = string.Empty;
            if (model.order.Count > 0)
            {
                ColumnName = model.columns[model.order[0].column].name;
                SortDir = model.order[0].dir;
            }

            List<string> ParamName = new List<string>();
            ParamName.Add("@CurrentPageNumber");
            ParamName.Add("@PageSize");
            ParamName.Add("@SortColumn");
            ParamName.Add("@SortDirection");
            ParamName.Add("@SearchText");

            List<object> ParamValue = new List<object>();
            ParamValue.Add(Convert.ToString(model.start)); // cur page no
            ParamValue.Add(Convert.ToString(model.length));// length
            ParamValue.Add(Convert.ToString(ColumnName));
            ParamValue.Add(Convert.ToString(SortDir)); // DESC
            ParamValue.Add(Convert.ToString(model.search.value));
            return await _repository.GetDataBySPAsync("STP_GoShare_GetListOfPayments", ParamName, ParamValue);
        }

        #endregion
    }
}
