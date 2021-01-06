using Microsoft.AspNetCore.Http;
using System.Linq;
using CRUD_App.General.Resources.Services;
using CRUD_App.Entity.Entity;
using FluentValidation;

namespace CRUD_App.Entity.EntityValidator
{

    public class BaseValidator<T> : AbstractValidator<T> where T : BaseEntity
    {
        public string message = "AlwaysTrue";
        public string methodType = "";
        public BaseValidator(IResourceService resourceService, IHttpContextAccessor httpContextAccessor)
        {
            methodType = httpContextAccessor.HttpContext.Request.Method;
            if (methodType.ToUpper().Equals("POST"))
            {
                RuleFor(u => u.CreatedBy).NotNull().GreaterThan(0);
            }
            else if (methodType.ToUpper().Equals("PUT"))
            {
                RuleFor(u => u.UpdatedBy).NotNull().GreaterThan(0);
            }
            else if (methodType.ToUpper().Equals("DELETE"))
            {
                RuleFor(u => u.UpdatedBy).NotNull().GreaterThan(0);
            }

            //RuleFor(r => r).Must((obj) => ValidateCommonFields(obj as PatientDto)).WithMessage(message);
        }

        public bool ValidateEntityState(BaseEntity entity)
        {
            bool[] op = { entity.IsDeleted };
            int count = op.Count(a => a);
            bool result = count == 1 ? true : false;
            return result;
        }
    }
}
