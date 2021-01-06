using AutoMapper;
using Go2Share.Data.Entity;
using Go2Share.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Go2Share.Services.ModelPaper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserEntity, User>()
                 .ForMember(dest => dest.Role, e => e.Ignore())
                 .ForMember(dest => dest.UserDrivingLicense, e => e.Ignore())
                 .ReverseMap();


        }
    }
    public class MapperProfile<TSource, TDestination> : Profile
    {
        public MapperProfile()
        {
            CreateMap<TSource, TDestination>().IgnoreAllNonExisting();
        }
    }
    public static class IMappingExpression
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
(this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }
    }
}
