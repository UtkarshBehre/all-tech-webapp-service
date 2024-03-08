using AutoMapper;

namespace all_tech_webapp_service.Providers
{
    public interface IAutoMapperProvider
    {
        public IMapper Mapper { get; }
    }
}
