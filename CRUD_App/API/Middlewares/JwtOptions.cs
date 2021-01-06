using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_App.API.Middlewares
{
    public class JwtOptions
    {
        public string secretKey { get; set; }
        public string issuer { get; set; }
        public bool validateLifetime { get; set; }
    }
}
