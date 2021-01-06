using CRUD_App.General.Datatable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_App.Services.Interfaces
{
    public interface IAdminService
    {
        Task<dynamic> GetDataBySPAsync(string SPName, List<string> ParamName = null, List<object> ParamValue = null);
        Task<dynamic> GetPageSoretedFilteredEarningsFromSP(DataTableAjaxPostModel model);
        Task<dynamic> GetPageSoretedFilteredPaymentsFromSP(DataTableAjaxPostModel model);
        Task<dynamic> GetAdminDashboard(int CustomerRoleID, int OwnerRoleID);
        Task<dynamic> GetListOfCancellationRequest(string StartDate, string EndDate);
        Task<dynamic> GetListOfRefundRequest(string StartDate, string EndDate, string FilePath);
    }
}
