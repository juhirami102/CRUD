using AutoMapper;
using Go2Share.Services.ModelPaper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Go2Share.Services.ModelMapper
{
    public class MapperFactory : IMapperFactory
    {
        public MapperFactory()
        {
            try
            {
                Mapper.Configuration.AssertConfigurationIsValid();
            }
            catch
            {
                AutoMapper.Mapper.Reset();
                Mapper.Initialize(cfg =>
                                      cfg.AddProfile<UserMapperProfile>());
            }
        }
        public TDestination Get<TSource, TDestination>(TSource source)
        {
            IMapper mapper = setConfig<TSource, TDestination>();
            return mapper.Map<TSource, TDestination>(source);
        }
        public IEnumerable<TDestination> GetList<TSource, TDestination>(IEnumerable<TSource> source)
        {
            IMapper mapper = setConfig<TSource, TDestination>();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
            //return Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }
        public List<TDestination> GetList<TSource, TDestination>(List<TSource> source)
        {
            IMapper mapper = setConfig<TSource, TDestination>();
            return mapper.Map<List<TSource>, List<TDestination>>(source);
            //return Mapper.Map<List<TSource>, List<TDestination>>(source);
        }
        public IMapper setConfig<TSource, TDestination>()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>().IgnoreAllNonExisting();//.ForMember(x => x.DestinationPropertyName, opt => opt.Ignore());
            });
            IMapper mapper = config.CreateMapper();
            return mapper;

        }

    }

}
