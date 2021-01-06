using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_App.API.Middlewares.Interface
{
    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
        string GetCurrentAsync();
    }
}
