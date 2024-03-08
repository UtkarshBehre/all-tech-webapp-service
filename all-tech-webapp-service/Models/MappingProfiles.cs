using all_tech_webapp_service.Models.ToDoItem;
using AutoMapper;

namespace all_tech_webapp_service.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ToDoItemCreateRequest, ToDoItemRecord>();
            CreateMap<ToDoItemCreateRequest, ToDoItemRecord>();
            CreateMap<ToDoItemUpdateRequest, ToDoItemRecord>();
            CreateMap<ToDoItemRecord, ToDoItemResponse>();
        }
    }
}
