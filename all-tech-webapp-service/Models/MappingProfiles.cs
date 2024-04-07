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
            CreateMap<TodoItemRecord, TodoItemResponse>();

            // Todo Group
            CreateMap<TodoGroupCreateRequest, TodoGroupRecord>();
            CreateMap<TodoGroupRequest, TodoGroupRecord>();
            CreateMap<TodoGroupUpdateRequest, TodoGroupRecord>();
            CreateMap<TodoGroupRecord, TodoGroupResponse>();

            // User Todo
            CreateMap<UserTodoCreateRequest, UserTodoRecord>();
            CreateMap<UserTodoRequest, UserTodoRecord>();
            CreateMap<UserTodoUpdateRequest, UserTodoRecord>();
            CreateMap<UserTodoRecord, UserTodoResponse>();

            // anti-pattern
            CreateMap<UserTodoResponse, UserTodoRequest>();

            // User
            CreateMap<UserCreateRequest, UserRecord>();
            CreateMap<UserRequest, UserRecord>();
            CreateMap<UserUpdateRequest, UserRecord>();
            CreateMap<UserRecord, UserResponse>();
        }
    }
}
