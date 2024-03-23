using AutoMapper;

namespace all_tech_webapp_service.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IMapper mapper, IEnumerable<TSource> source)
        {
            foreach (var item in source)
            {
                yield return mapper.Map<TSource, TDestination>(item);
            }
        }
    }
}
