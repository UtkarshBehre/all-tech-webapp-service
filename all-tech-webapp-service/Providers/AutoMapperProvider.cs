using all_tech_webapp_service.Models;
using AutoMapper;

namespace all_tech_webapp_service.Providers
{
    public class AutoMapperProvider : IAutoMapperProvider
    {
        public IMapper Mapper { get; private set; }

        public AutoMapperProvider() 
        { 
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfiles>();
            });

            Mapper = config.CreateMapper();
        }
    }
}
