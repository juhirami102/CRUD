using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using CRUD_App.Entity.Entity;

namespace CRUD_App.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserEntity>> GetPagedSortedFilteredListAsync(int start, int length, string orderColumnName, ListSortDirection order, string searchValue);
        Task<List<UserEntity>> GetAllUser();
        Task<UserEntity> GetUserById(int id);
        Task<UserEntity> AddUser(UserEntity entityUser);
        Task<UserEntity> UpdateUser(UserEntity entityUser);
        Task<UserEntity> DeleteUser(int id);
        Task<int> GetRecordsFilteredAsync(string searchValue);
        Task<int> GetRecordsTotalAsync();
       // User Authenticate(string username, string password);
        //Added by Sumit later on we should remove above method once it finalised
        UserEntity Authenticate(string username, string password, int RoleId);

        UserEntity GetUserByMobileNo(string MobileNumber);

        bool IsUserExist(UserEntity entityUser);
        Task<UserDetailEntity> AddUserDetail(UserDetailEntity entityUser);
        Task<UserDetailEntity> UpdateUserDetail(UserDetailEntity entityUser);
        bool IsExistUserByDeviceToken(string DeviceToken, int RoleId);
        UserDetailEntity UpdatePartialDeviceToken(UserDetailEntity entityUserDetail);
        UserDetailEntity IsReferalCodeExist(string referalCode);

        Task<UserEntity> ChangePassword(UserEntity entityUser);//By Prasad 
        bool IsUserIdPasswordExist(UserEntity entityUser); 
        //Task<UserDetailEntity> UpdatePartialDeviceToken(UserDetailEntity entityUserDetail);
        //UserEntity GetUserByFilter(Expression<Func<UserEntity, bool>> predicate);
    }
}
