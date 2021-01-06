using Go2Share.Core.Repositories;
using Go2Share.Core.UnitOfWork;
using Go2Share.Data.Entity;
using Go2Share.Entity.Entity;
using Go2Share.General;
using Go2Share.General.Datatable;
using Go2Share.General.Resources.Common;
using Go2Share.General.Resources.Services;
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
   public class FuelService : IFuelService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperFactory _mapperFactory;
        private readonly IResourceService _messageService;
        private IRepository<FuelMaster> _repository { get; set; }
        #endregion

        #region Constructor
        public FuelService(IUnitOfWork unitOfWork, IMapperFactory mapperFactory, IResourceService messageService)
        {
            _unitOfWork = unitOfWork;
            _mapperFactory = mapperFactory;
            _repository = _unitOfWork.GetRepository<FuelMaster>();
            _messageService = messageService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Purpose : Create new Fuel Capacity
        /// </summary>
        /// <param name="entityFuel"></param>
        /// <returns></returns>
        public async Task<FuelMasterEntity>AddFuel(FuelMasterEntity entityFuel)
        {
            entityFuel.Ipaddress = Helper.GetIPAddress(Helper.httpRequest);
            //var isExist = _repository.Exists(x => x.FuelCapacity.ToLowerInvariant() == entityFuel.FuelCapacity.ToLowerInvariant() && !x.IsDeleted);
            //if (isExist)
            //    throw new Exception(_messageService.GetString("Exist"));

            FuelMaster oFuel = _mapperFactory.Get<FuelMasterEntity, FuelMaster>(entityFuel);
            _repository.AddAsync(oFuel);
            var oResult = await _unitOfWork.SaveChangesAsync();
            if (oFuel.FuelId == 0) throw new Exception(_messageService.GetString("AddFailed"));
            return _mapperFactory.Get<FuelMaster, FuelMasterEntity>(oFuel);
        }

        /// <summary>
        /// Purpose : Delete Fuel Capacity by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuelMasterEntity> DeleteFuel(int id)
        {
            var entityFuel = _repository.GetEntity(x => x.FuelId == id);
            if (entityFuel == null)
                return null;

            FuelMaster mFuel = await _repository.GetNoTrackingAsync(entityFuel.FuelId);
            _repository.Remove(entityFuel);
            var oResult = await _unitOfWork.SaveChangesAsync();
            return _mapperFactory.Get<FuelMaster, FuelMasterEntity>(mFuel);
        }

        /// <summary>
        /// Purpose : Get all Fuel Capacity.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FuelMasterEntity>> GetAllFuel()
        {
            var AllFuel = await _repository.GetAllAsync();
            if (AllFuel.Count() > 0)
                return _mapperFactory.GetList<FuelMaster, FuelMasterEntity>(AllFuel);
            else
                return null;
        }

        /// <summary>
        /// Purpose : Get Fuel Capacity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuelMasterEntity> GetFuelById(int id)
        {
            var Fuel = await _repository.GetAsync(id);
            if (Fuel == null)
                return null;
            else
                return _mapperFactory.Get<FuelMaster, FuelMasterEntity>(Fuel);
        }

        /// <summary>
        /// Purpose : Update Fuel Capacity
        /// </summary>
        /// <param name="entityFuel"></param>
        /// <returns></returns>
        public async Task<FuelMasterEntity> UpdateFuel(FuelMasterEntity entityFuel)
        {
            var oFuelOld = _repository.Get(entityFuel.FuelId);

          
          
            FuelMaster oFuel = _mapperFactory.Get<FuelMasterEntity, FuelMaster>(entityFuel);
            if (oFuelOld != null)
            {
                oFuel.FuelCapacity = !string.IsNullOrEmpty(entityFuel.FuelCapacity) ? entityFuel.FuelCapacity : oFuelOld.FuelCapacity;
                oFuel.UpdatedBy = entityFuel.UpdatedBy != 0 ? entityFuel.UpdatedBy : oFuelOld.UpdatedBy;
                oFuel.UpdatedDate = entityFuel.UpdatedDate != null ? entityFuel.UpdatedDate : oFuelOld.UpdatedDate;
                oFuel.IsDeleted = entityFuel.IsDeleted ? entityFuel.IsDeleted : false;
                oFuel.DeletedBy = entityFuel.DeletedBy != null ? entityFuel.DeletedBy : oFuelOld.DeletedBy;
                oFuel.DeletedDate = entityFuel.DeletedDate != null ? entityFuel.DeletedDate : oFuelOld.DeletedDate;
                oFuel.Ipaddress = !string.IsNullOrEmpty(entityFuel.Ipaddress) ? entityFuel.Ipaddress : Helper.GetIPAddress(Helper.httpRequest);

                _repository.UpdateAsync(oFuel);
                var oResult = await _unitOfWork.SaveChangesAsync();
                if (oFuel.FuelId == 0) throw new Exception(entityFuel.IsDeleted ? _messageService.GetString("DeleteFailed") : _messageService.GetString("UpdateFailed"));
                return _mapperFactory.Get<FuelMaster, FuelMasterEntity>(oFuel);
            }
                return _mapperFactory.Get<FuelMaster, FuelMasterEntity>(oFuelOld);//
        }

        /// <summary>
        /// Purpose : Get paged,sorted and filtered records as per model passed
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<dynamic> GetPageSoretedFilteredListFromSP(string UserId, DataTableAjaxPostModel model)
        {
            string ColumnName = string.Empty;
            string SortDir = string.Empty;
            if (model.order.Count > 0)
            {
                ColumnName = model.columns[model.order[0].column].name;
                SortDir = model.order[0].dir;
            }

            List<string> ParamName = new List<string>();
            ParamName.Add("@UserId");
            ParamName.Add("@CurrentPageNumber");
            ParamName.Add("@PageSize");
            ParamName.Add("@SortColumn");
            ParamName.Add("@SortDirection");
            ParamName.Add("@SearchText");
            ParamName.Add("@ColFuelId");
            ParamName.Add("@ColFuelCapacity");
            ParamName.Add("@ColCreatedDate");
            ParamName.Add("@ColUpdatedDate");
            ParamName.Add("@ColIsActive");

            List<object> ParamValue = new List<object>();
            ParamValue.Add(Convert.ToString(UserId));// 1
            ParamValue.Add(Convert.ToString(model.start)); // cur page no
            ParamValue.Add(Convert.ToString(model.length));// length
            ParamValue.Add(Convert.ToString(ColumnName));
            ParamValue.Add(Convert.ToString(SortDir)); // DESC
            ParamValue.Add(Convert.ToString(model.search.value));
            ParamValue.Add(model.columns.Exists(x => x.name.ToLower() == "fuelid") ? Convert.ToString(model.columns.Where(x => x.name.ToLower() == "fuelid").FirstOrDefault().search.value) : string.Empty);
            ParamValue.Add(model.columns.Exists(x => x.name.ToLower() == "fuelcapacity") ? Convert.ToString(model.columns.Where(x => x.name.ToLower() == "fuelcapacity").FirstOrDefault().search.value) : string.Empty);
            ParamValue.Add(model.columns.Exists(x => x.name.ToLower() == "createddate") ? Convert.ToString(model.columns.Where(x => x.name.ToLower() == "createddate").FirstOrDefault().search.value) : string.Empty);
            ParamValue.Add(model.columns.Exists(x => x.name.ToLower() == "updateddate") ? Convert.ToString(model.columns.Where(x => x.name.ToLower() == "updateddate").FirstOrDefault().search.value) : string.Empty);
            ParamValue.Add(model.columns.Exists(x => x.name.ToLower() == "isactive") ? Convert.ToString(model.columns.Where(x => x.name.ToLower() == "isactive").FirstOrDefault().search.value) : string.Empty); 
            DataTable dtAllFuel = await _repository.GetDataBySPAsync("STP_GoShare_GetAllFuelCapacity", ParamName, ParamValue);

            var TotalRecord = 0;
            var TotalCount = 0;
            if (dtAllFuel != null && dtAllFuel.Rows.Count > 0)
            {
                TotalRecord = Convert.ToInt32(dtAllFuel.Rows[0]["TotalRecord"]);
                TotalCount = Convert.ToInt32(dtAllFuel.Rows[0]["TotalCount"]);
            }

            dynamic result = new ExpandoObject();
            result.AllFuel = dtAllFuel.DataTableToList<FuelMasterEntity>();
            result.TotalRecord = TotalRecord;
            result.TotalCount = TotalCount;

            return result;
        }

        /// <summary>
        /// Purpose : get fuel detail with sepecific search value
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public async Task<int> GetRecordsFilteredAsync(string searchValue)
        {
            return await _repository.GetRecordsFilteredAsync(searchValue);
        }
        /// <summary>
        /// Purpose : To get count of records.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetRecordsTotalAsync()
        {
            return await _repository.GetRecordsTotalAsync();
        }

        public async Task<IEnumerable<FuelMasterEntity>> GetPagedSortedFilteredListAsync(int start, int length, string orderColumnName, ListSortDirection order, string searchValue)
        {
            var allFuel = await _repository.GetPagedSortedFilteredListAsync(start, length, orderColumnName, order, searchValue);
            return _mapperFactory.GetList<FuelMaster, FuelMasterEntity>(allFuel);
        }
        #endregion

        //Added by Prasad 
        public bool IsFuelExist(FuelMasterEntity entityBrandMaster)
        {
            var isExist = _repository.Exists(x => (entityBrandMaster != null && x.FuelId != entityBrandMaster.FuelId && x.FuelCapacity.ToLowerInvariant() == entityBrandMaster.FuelCapacity.ToLowerInvariant() && x.IsDeleted == false));
            if (isExist)
                return true;

            return false;
        }
    }
}
