using System;
using System.Collections.Generic;
using System.Text;

namespace Go2Share.Services.ModelMapper
{
    public interface IMapperFactory
    {
        TDestination Get<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> GetList<TSource, TDestination>(IEnumerable<TSource> source);

        List<TDestination> GetList<TSource, TDestination>(List<TSource> source);
    }
}
