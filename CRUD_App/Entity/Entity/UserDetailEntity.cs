using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CRUD_App.Entity.Entity
{
    public class UserDetailEntity
    {
        public int UserDetailId { get; set; }
        public int UserId { get; set; }
        public string UserDeviceToken { get; set; }
        public string ReferCode { get; set; }
        public decimal WalletAmount { get; set; }
        public string Ipaddress { get; set; }
       
    }
}
