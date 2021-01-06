using System;
using System.Collections.Generic;
using System.Text;

namespace Go2Share.Entity.Entity
{
   public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public General.Enums.GeneralEnums.RoleType Role { get; set; }

        public string DeviceToken { get; set; }
    }
}
