
using CRUD_App.Entity;
using CRUD_App.General.Resources.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_App.Entity.EntityValidator
{
    public class UserValidator : BaseValidator<UserEntity>
    {
        //private readonly Go2ShareContext _DAPSFAPPDBContext;
        private readonly IResourceService _messageService;
        private IHttpContextAccessor _httpContextAccessor;
        public UserValidator(IHttpContextAccessor httpContextAccessor, IResourceService messageService) : base(messageService, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _messageService = messageService;

            methodType = _httpContextAccessor.HttpContext.Request.Method;
            if (methodType.ToUpper().Equals("PUT") || methodType.ToUpper().Equals("DELETE"))
            {
                RuleFor(u => u.UserId).GreaterThan(0).NotNull();

            }
            //methodType.ToUpper().Equals("PUT")
            if (methodType.ToUpper().Equals("POST"))
            {
                RuleFor(u => u.RoleId).GreaterThan(0).NotNull();
                RuleFor(p => p.FirstName).MaximumLength(50).NotNull();
                RuleFor(p => p.EmailAddress).EmailAddress().MaximumLength(50).NotNull();
                RuleFor(p => p.Password).MaximumLength(50).NotNull();
                RuleFor(p => p.MobileNumber).MaximumLength(20);
            }
        }

    }
}
