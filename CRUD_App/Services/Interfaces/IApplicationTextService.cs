using CRUD_App.Entity.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_App.Services.Interfaces
{
    public interface IApplicationTextService
    {
        Task<List<ApplicationTextEntity>> GetAllApplicationText();
        Task<ApplicationTextEntity> GetApplicationTextById(int id);
    }
}
