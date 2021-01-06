using CRUD_App.Entity.Entity;
using System.Threading.Tasks;

namespace CRUD_App.Services.Interfaces
{
    public interface ISettingService
    {
        Task<dynamic> GetAllSetting();
        Task<SettingMasterEntity> AddUpdateSetting(SettingMasterEntity entitySetting, int UserId);
    }
}
