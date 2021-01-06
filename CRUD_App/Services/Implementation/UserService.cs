using Go2Share.Core.Repositories;
using Go2Share.Core.UnitOfWork;
using Go2Share.Data.Entity;
using Go2Share.Entity.Entity;
using Go2Share.General.Resources.Common;
using Go2Share.General.Resources.Services;
using Go2Share.Services.Interfaces;
using Go2Share.Services.ModelMapper;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Go2Share.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperFactory _mapperFactory;
        private readonly IResourceService _messageService;
        //private readonly IAuditService _auditService;
        private IRepository<User> _repository { get; set; }
        private IRepository<RoleMaster> _repositoryRole { get; set; }
        private IRepository<UserDetail> _repositoryUD { get; set; }
        private IRepository<ReferalLogs> _repositoryRefLog { get; set; }
        public UserService(IUnitOfWork unitOfWork, IMapperFactory mapperFactory, IResourceService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapperFactory = mapperFactory;
            _repository = _unitOfWork.GetRepository<User>();
            _repositoryRole = _unitOfWork.GetRepository<RoleMaster>();
            _repositoryUD = _unitOfWork.GetRepository<UserDetail>();
            _repositoryRefLog = _unitOfWork.GetRepository<ReferalLogs>();
            _messageService = messageService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityUser"></param>
        /// <returns></returns>
        public async Task<UserEntity> AddUser(UserEntity entityUser)
        {
            var _roleIdExist = _repositoryRole.Exists(x => x.RoleId == entityUser.RoleId);
            if (!_roleIdExist)
                return null;

            //Set Password
            entityUser.Password = Helper.GenerateSHA256String(entityUser.Password);

            var isExist = _repository.Exists(x => (x.EmailAddress.ToLowerInvariant() == entityUser.EmailAddress.ToLowerInvariant() && x.RoleId == entityUser.RoleId));
            if (isExist)
                return null;

            User oUser = _mapperFactory.Get<UserEntity, User>(entityUser);
            _repository.AddAsync(oUser);
            var oResult = await _unitOfWork.SaveChangesAsync();
            if (oUser.UserId == 0) return null; //throw new Exception(_messageService.GetString("AddFailed"));
            return _mapperFactory.Get<User, UserEntity>(oUser);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityUser"></param>
        /// <returns></returns>
        public async Task<UserEntity> DeleteUser(int id)
        {
            var entityUser = _repository.GetEntity(x => x.UserId == id);
            if (entityUser == null)
                return null;

            User mUser = await _repository.GetNoTrackingAsync(entityUser.UserId);
            _repository.Remove(entityUser);
            var oResult = await _unitOfWork.SaveChangesAsync();
            return _mapperFactory.Get<User, UserEntity>(mUser);
        }
        /// <summary>
        /// To Get All Users List
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserEntity>> GetAllUser()
        {
            var AllUser = await _repository.GetAllAsync();
            if (AllUser.Count() > 0)
                return _mapperFactory.GetList<User, UserEntity>(AllUser);
            else
                return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public async Task<int> GetRecordsFilteredAsync(string searchValue)
        {
            return await _repository.GetRecordsFilteredAsync(searchValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecordsTotalAsync()
        {
            return await _repository.GetRecordsTotalAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserEntity> GetUserById(int id)
        {
             
                var User = await _repository.GetAsync(id);
                return _mapperFactory.Get<User, UserEntity>(User);
           

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityUser"></param>
        /// <returns></returns>
        public async Task<UserEntity> UpdateUser(UserEntity entityUser)
        {
            if (!string.IsNullOrEmpty(entityUser.Password))
                entityUser.Password = Helper.GenerateSHA256String(entityUser.Password);
            var oUserOld = _repository.Get(entityUser.UserId);


            User oUser = _mapperFactory.Get<UserEntity, User>(entityUser);
            oUser.FirstName = !string.IsNullOrEmpty(entityUser.FirstName) ? entityUser.FirstName : oUserOld.FirstName;
            oUser.LastName = !string.IsNullOrEmpty(entityUser.LastName) ? entityUser.LastName : oUserOld.LastName;
            oUser.EmailAddress = !string.IsNullOrEmpty(entityUser.EmailAddress) ? entityUser.EmailAddress : oUserOld.EmailAddress;
            oUser.MobileNumber = !string.IsNullOrEmpty(entityUser.MobileNumber) ? entityUser.MobileNumber : oUserOld.MobileNumber;
            oUser.ProfileImage = !string.IsNullOrEmpty(entityUser.ProfileImage) ? entityUser.ProfileImage : oUserOld.ProfileImage;
            oUser.UpdatedBy = entityUser.UpdatedBy != 0 ? entityUser.UpdatedBy : oUserOld.UpdatedBy;
            oUser.UpdatedDate = entityUser.UpdatedDate != null ? entityUser.UpdatedDate : oUserOld.UpdatedDate;
            oUser.Password = entityUser.Password != null ? entityUser.Password : oUserOld.Password;
            oUser.RoleId = entityUser.RoleId != 0 ? entityUser.RoleId : oUserOld.RoleId;
            oUser.IsDeleted = entityUser.IsDeleted ? entityUser.IsDeleted : false;
            oUser.DeletedBy = entityUser.DeletedBy != null ? entityUser.DeletedBy : null;
            oUser.DeletedDate = entityUser.DeletedDate != null ? entityUser.DeletedDate : null;
            oUser.IsAgree = entityUser.IsAgree ? entityUser.IsAgree : false;
            oUser.IsActive = entityUser.IsActive != null && entityUser.IsActive == true ? true : false;

            _repository.UpdateAsync(oUser);
            var oResult = await _unitOfWork.SaveChangesAsync();
            if (oUser.UserId == 0) throw new Exception(entityUser.IsDeleted ? _messageService.GetString("DeleteFailed") : _messageService.GetString("UpdateFailed"));
            return _mapperFactory.Get<User, UserEntity>(oUser);
        }
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;


            var _password = Helper.GenerateSHA256String(password);
            var User = _repository.GetEntity(x => x.EmailAddress.ToLowerInvariant() == username.ToLowerInvariant() && x.Password == _password);
            // check if username exists // check if password is correct
            //var getUserRole=_repository.get

            if (User == null)
                return null;

            return User;
        }
        public UserEntity Authenticate(string username, string password, int RoleId)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;


            var _password = Helper.GenerateSHA256String(password);
            var User = _repository.GetEntity(x => (x.EmailAddress.ToLowerInvariant() == username.ToLowerInvariant() || x.MobileNumber.ToLowerInvariant() == username.ToLowerInvariant()) && x.Password == _password
                        && x.RoleId == RoleId);
            // check if username exists // check if password is correct
            //var getUserRole=_repository.get

            if (User == null)
                return null;
            return _mapperFactory.Get<User, UserEntity>(User);
        }
        public async Task<IEnumerable<UserEntity>> GetPagedSortedFilteredListAsync(int start, int length, string orderColumnName, ListSortDirection order, string searchValue)
        {
            var allUser = await _repository.GetPagedSortedFilteredListAsync(start, length, orderColumnName, order, searchValue);
            return _mapperFactory.GetList<User, UserEntity>(allUser);
        }

        public UserEntity GetUserByMobileNo(string MobileNumber)
        {
            var entityUser = _repository.GetEntity(x => x.MobileNumber == MobileNumber);
            if (entityUser != null)
                return _mapperFactory.Get<User, UserEntity>(entityUser);
            else
                return null;
        }
        public bool IsUserExist(UserEntity entityUser)
        {
            var isExist = _repository.Exists(x => (x.EmailAddress.ToLowerInvariant() == entityUser.EmailAddress.ToLowerInvariant() && x.RoleId == entityUser.RoleId));
            if (isExist)
                return true;

            return false;
        }

        public async Task<UserDetailEntity> AddUserDetail(UserDetailEntity entityUserDetail)
        {
             
                UserDetail _Data = _repositoryUD.Get(x => x.UserId == entityUserDetail.UserId);
                if (_Data != null)
                {
                    _Data.UserDeviceToken = entityUserDetail.UserDeviceToken;
                    _repositoryUD.UpdateAsync(_Data);
                    var oResultD = _unitOfWork.SaveChanges();
                    return _mapperFactory.Get<UserDetail, UserDetailEntity>(_Data);
                }
            
            entityUserDetail.WalletAmount = 0;
            entityUserDetail.ReferCode = Helper.GetReferralCode();
            UserDetail oUser = _mapperFactory.Get<UserDetailEntity, UserDetail>(entityUserDetail);
            _repositoryUD.AddAsync(oUser);
            var oResult = await _unitOfWork.SaveChangesAsync();
            if (oUser.UserId == 0) throw new Exception(_messageService.GetString("AddFailed"));
            return _mapperFactory.Get<UserDetail, UserDetailEntity>(oUser);
        }

        public bool IsExistUserByDeviceToken(string DeviceToken, int RoleId)
        {
            var isExistUser = false;
           
                //var data = _repositoryUD.Get(x => x.UserDeviceToken.ToLowerInvariant() == DeviceToken.ToLowerInvariant());
                using (var context = new Go2ShareContext())
                {
                    var _Data = (from a in context.UserDetail
                                 join
                                 b in context.User on a.UserId equals b.UserId
                                 join
                                    c in context.RoleMaster on b.RoleId equals c.RoleId
                                 where a.UserDeviceToken == DeviceToken &&
                                 c.RoleId == RoleId
                                 select
                                    new { a.UserId, a.UserDeviceToken }).ToList();
                    if (_Data != null && _Data.Count > 0)
                    {
                        isExistUser = true;
                    }
                }
            
            return isExistUser;
        }
        public async Task<UserDetailEntity> UpdateUserDetail(UserDetailEntity entityUserDetail)
        {
            UserDetail oUserDetail = new UserDetail();

            oUserDetail = _mapperFactory.Get<UserDetailEntity, UserDetail>(entityUserDetail);
            _repositoryUD.UpdateAsync(oUserDetail);
            var oResult = await _unitOfWork.SaveChangesAsync();
           
                var dd = UpdatePartialDeviceToken(entityUserDetail);
            
            return _mapperFactory.Get<UserDetail, UserDetailEntity>(oUserDetail);
        }
        public UserDetailEntity UpdatePartialDeviceToken(UserDetailEntity entityUserDetail)
        {
            var data = _repositoryUD.Get(x => x.UserDeviceToken.ToLowerInvariant() == entityUserDetail.UserDeviceToken.ToLowerInvariant());
            if (data != null)
            {
                data.UserDeviceToken = string.Empty;
                _repositoryUD.UpdateAsync(data);
                var oResultD = _unitOfWork.SaveChanges();
                return _mapperFactory.Get<UserDetail, UserDetailEntity>(data);
            }
            return null;
        }
        public UserDetailEntity IsReferalCodeExist(string referalCode)
        {
           
                var _UserDetail = _repositoryUD.Get(x => x.ReferCode.ToLowerInvariant() == referalCode.ToLowerInvariant());
                return _mapperFactory.Get<UserDetail, UserDetailEntity>(_UserDetail);
            
        }

        public async Task<ReferalLogsEntity> AddReferalLog(ReferalLogsEntity entityReferal)
        {
           
                ReferalLogs oUserRefLog = _mapperFactory.Get<ReferalLogsEntity, ReferalLogs>(entityReferal);
                _repositoryRefLog.AddAsync(oUserRefLog);
                var oResult = await _unitOfWork.SaveChangesAsync();
                if (oUserRefLog.ReferalUserLogId == 0) throw new Exception(_messageService.GetString("AddFailed"));
                return _mapperFactory.Get<ReferalLogs, ReferalLogsEntity>(oUserRefLog);
           
        }

        public async Task<UserEntity> ChangePassword(UserEntity entityUser)
        {
            //var pwd=    Helper.GenerateSHA256String(entityUser.Password);
            var oUserOld = _repository.Get(entityUser.UserId);
            User oUser = _mapperFactory.Get<UserEntity, User>(entityUser);
            if (oUserOld != null)
            {
                oUser.FirstName = !string.IsNullOrEmpty(entityUser.FirstName) ? entityUser.FirstName : oUserOld.FirstName;
                oUser.LastName = !string.IsNullOrEmpty(entityUser.LastName) ? entityUser.LastName : oUserOld.LastName;
                oUser.EmailAddress = !string.IsNullOrEmpty(entityUser.EmailAddress) ? entityUser.EmailAddress : oUserOld.EmailAddress;
                oUser.MobileNumber = !string.IsNullOrEmpty(entityUser.MobileNumber) ? entityUser.MobileNumber : oUserOld.MobileNumber;
                oUser.ProfileImage = !string.IsNullOrEmpty(entityUser.ProfileImage) ? entityUser.ProfileImage : oUserOld.ProfileImage;
                oUser.UpdatedBy = entityUser.UpdatedBy != 0 ? entityUser.UpdatedBy : oUserOld.UpdatedBy;
                oUser.UpdatedDate = entityUser.UpdatedDate != null ? entityUser.UpdatedDate : oUserOld.UpdatedDate;
                oUser.Password = entityUser.Password != null ? entityUser.Password : oUserOld.Password;
                oUser.RoleId = entityUser.RoleId != 0 ? entityUser.RoleId : oUserOld.RoleId;
                oUser.IsDeleted = entityUser.IsDeleted ? entityUser.IsDeleted : false;
                oUser.DeletedBy = entityUser.DeletedBy != null ? entityUser.DeletedBy : null;
                oUser.DeletedDate = entityUser.DeletedDate != null ? entityUser.DeletedDate : null;
                oUser.IsAgree = entityUser.IsAgree ? entityUser.IsAgree : false;
                oUser.IsActive = entityUser.IsActive != null && entityUser.IsActive == true ? true : false;
                oUser.Password = entityUser.Password; ;

                _repository.UpdateAsync(oUser);
                var oResult = await _unitOfWork.SaveChangesAsync();
                if (oUser.UserId == 0) throw new Exception(entityUser.IsDeleted ? _messageService.GetString("DeleteFailed") : _messageService.GetString("UpdateFailed"));
                return _mapperFactory.Get<User, UserEntity>(oUser);
            }
            return _mapperFactory.Get<User, UserEntity>(oUserOld);
        }
        public bool IsUserIdPasswordExist(UserEntity entityUser)
        {
            Go2ShareContext db = new Go2ShareContext();
            //User isExist = (from data in db.User where data.UserId ==entityUser.UserId && data.Password == entityUser.Password select data).SingleOrDefault();
            var isExist = _repository.Exists(x => (entityUser != null && x.UserId != entityUser.UserId && x.Password.ToLowerInvariant() == entityUser.Password.ToLowerInvariant() && x.IsDeleted == false));

            if (isExist == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
