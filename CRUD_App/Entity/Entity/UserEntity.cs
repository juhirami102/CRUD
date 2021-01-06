using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CRUD_App.Entity.Entity
{
    public class UserEntity : BaseEntity
    {

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        //public DateTime? CreatedDate { get; set; }
        public string ProfileImage { get; set; }
        public bool? IsActive { get; set; }

        public bool IsAgree { get; set; }

        public string OTP { get; set; }

        public int RoleId { get; set; }
        public string UserDeviceToken { get; set; }
        public string ReferalCode { get; set; }
        //[IgnoreDataMember]
        //public RoleMasterEntity Role { get; set; }
      


    }

    public class UserLoggedInModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public int RoleId { get; set; }
    }


}
