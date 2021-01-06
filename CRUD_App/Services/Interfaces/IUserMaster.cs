using CRUD_App.Entity;
using CRUD_App.Entity.Entity;
using CRUD_App.General.Datatable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_App.Services.Interfaces
{
    public interface IUserMasterService
    {
        Task<IEnumerable<UserMasterEntity>> GetPagedSortedFilteredListAsync(int start, int length, string orderColumnName, ListSortDirection order, string searchValue);
        Task<List<UserMasterEntity>> GetAllUser();
        Task<UserMasterEntity> GetUserById(int id);
        Task<UserMasterEntity> AddUser(UserMasterEntity entityUser);
        Task<UserMasterEntity> UpdateUser(UserMasterEntity entityUser);
        Task<UserMasterEntity> DeleteUser(int id);
        Task<int> GetRecordsFilteredAsync(string searchValue);
        Task<int> GetRecordsTotalAsync();
        Task<dynamic> GetPageSoretedFilteredListFromSP(string UserId, DataTableAjaxPostModel model);
          bool IsUserExist(UserMasterEntity entityBrandMaster);
    }
}
