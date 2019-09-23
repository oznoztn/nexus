using System.Collections.Generic;
using AutoMapper;

namespace Nexus.Service.Extensions
{
    public static class AutoMapperExtensions
    {
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        public static IEnumerable<TDestination> MapTo<TDestination>(this IEnumerable<object> source)
        {
            return Mapper.Map<IEnumerable<TDestination>>(source);
        }

        //public static List<TDestination> MapTo<TDestination>(this IEnumerable source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException();

        //    return (List<TDestination>)Mapper.Map(source, source.GetType(), typeof(List<TDestination>));
        //}

        //public static TDestination MapTo<TDestination>(this object source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException();

        //    return (TDestination)Mapper.Map(source, source.GetType(), typeof(TDestination));
        //}
    }
}
