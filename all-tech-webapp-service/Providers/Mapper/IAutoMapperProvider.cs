using AutoMapper;

namespace all_tech_webapp_service.Providers.Mapper
{
    public interface IAutoMapperProvider
    {
        public IMapper Mapper { get; }
    }
}
