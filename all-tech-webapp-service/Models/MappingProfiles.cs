using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Models.Todo.UserTodo;
using all_tech_webapp_service.Models.User;
using AutoMapper;

namespace all_tech_webapp_service.Models
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Todo Item

            CreateMap<TodoItemCreateRequest, TodoItemRecord>();
            CreateMap<TodoItemRequest, TodoItemRecord>();
            CreateMap<TodoItemUpdateRequest, TodoItemRecord>();
            CreateMap<TodoItemRecord, TodoItemResponse>()
                .ForMember(x => x.CompletedAt, opt => opt.MapFrom(x => x.CompletedAt != null ? DateTimeOffset.FromUnixTimeSeconds(x.CompletedAt.Value) : (DateTimeOffset?)null))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt)))
                .ForMember(x => x.LastModifiedAt, opt => opt.MapFrom(item => item.LastModifiedAt != null ? DateTimeOffset.FromUnixTimeSeconds(item.LastModifiedAt.Value) : (DateTimeOffset?)null));

            // Todo Group
            CreateMap<TodoGroupCreateRequest, TodoGroupRecord>();
            CreateMap<TodoGroupRequest, TodoGroupRecord>();
            CreateMap<TodoGroupUpdateRequest, TodoGroupRecord>();
            CreateMap<TodoGroupRecord, TodoGroupResponse>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt)))
                .ForMember(x => x.LastModifiedAt, opt => opt.MapFrom(item => item.LastModifiedAt != null ? DateTimeOffset.FromUnixTimeSeconds(item.LastModifiedAt.Value) : (DateTimeOffset?)null)); ;

            // User Todo
            CreateMap<UserTodoCreateRequest, UserTodoRecord>();
            CreateMap<UserTodoRequest, UserTodoRecord>();
            CreateMap<UserTodoUpdateRequest, UserTodoRecord>();
            CreateMap<UserTodoRecord, UserTodoResponse>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt)))
                .ForMember(x => x.LastModifiedAt, opt => opt.MapFrom(item => item.LastModifiedAt != null ? DateTimeOffset.FromUnixTimeSeconds(item.LastModifiedAt.Value) : (DateTimeOffset?)null));
            
            // User
            CreateMap<UserCreateRequest, UserRecord>();
            CreateMap<UserRequest, UserRecord>();
            CreateMap<UserUpdateRequest, UserRecord>();
            CreateMap<UserRecord, UserResponse>()
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => DateTimeOffset.FromUnixTimeSeconds(x.CreatedAt)))
                .ForMember(x => x.LastModifiedAt, opt => opt.MapFrom(item => item.LastModifiedAt != null ? DateTimeOffset.FromUnixTimeSeconds(item.LastModifiedAt.Value) : (DateTimeOffset?)null));
        }
    }
}
