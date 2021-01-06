using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CRUD_App.Entity.Entity
{
   public class UserMasterEntity
    {
        [Display(Name = "User Id")]
        public int UserId { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }
        [Display(Name = "Updated By")]
        public int? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
      
    }
}
