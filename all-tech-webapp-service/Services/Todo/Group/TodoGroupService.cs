using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Models.Todo.UserTodo;
using all_tech_webapp_service.Providers.Mapper;
using all_tech_webapp_service.Providers.Token;
using all_tech_webapp_service.Repositories.Todo.TodoGroupRepository;
using all_tech_webapp_service.Repositories.Todo.UserTodo;
using all_tech_webapp_service.Repositories.User;

namespace all_tech_webapp_service.Services.Todo.Group
{
    public class TodoGroupService : ITodoGroupService
    {
        private readonly ITodoGroupRepository _todoGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserTodoRepository _userTodoRepository;
        private readonly ITokenHandlerProvider _tokenHandlerProvider;
        private readonly IAutoMapperProvider _autoMapperProvider;

        /// <summary>
        /// Contrustor for TodoGroupService
        /// </summary>
        /// <param name="todoGroupRepository"></param>
        /// <param name="autoMapperProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TodoGroupService(ITodoGroupRepository todoGroupRepository, IUserRepository userRepository, IUserTodoRepository userTodoRepository, ITokenHandlerProvider tokenHandlerProvider, IAutoMapperProvider autoMapperProvider)
        {
            _todoGroupRepository = todoGroupRepository ?? throw new ArgumentNullException(nameof(todoGroupRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userTodoRepository = userTodoRepository ?? throw new ArgumentNullException(nameof(userTodoRepository));
            _tokenHandlerProvider = tokenHandlerProvider ?? throw new ArgumentNullException(nameof(tokenHandlerProvider));
            _autoMapperProvider = autoMapperProvider ?? throw new ArgumentNullException(nameof(autoMapperProvider));
        }

        public async Task<TodoGroupResponse> CreateTodoGroup(TodoGroupCreateRequest todoGroupCreateRequest)
        {
            var todoGroupRecord = _autoMapperProvider.Mapper.Map<TodoGroupRecord>(todoGroupCreateRequest);
            var googleId = _tokenHandlerProvider.GetSubFromToken();
            var userRecord = await _userRepository.GetUserByGoogleId(googleId);
            todoGroupRecord.CreatedBy = userRecord.Id;
            todoGroupRecord.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            todoGroupRecord = await _todoGroupRepository.CreateTodoGroup(todoGroupRecord);
            var todoGroupResponse = _autoMapperProvider.Mapper.Map<TodoGroupResponse>(todoGroupRecord);
            return todoGroupResponse;
        }

        public async Task<TodoGroupResponse> GetTodoGroup(Guid id)
        {
            var todoGroupRecord = await _todoGroupRepository.GetTodoGroup(id);
            var todoGroupResponse = _autoMapperProvider.Mapper.Map<TodoGroupResponse>(todoGroupRecord);
            return todoGroupResponse;
        }

        public async Task<List<TodoGroupResponse>> GetTodoGroups(List<Guid> ids)
        {
            var todoGroupRecords = await _todoGroupRepository.GetTodoGroups(ids);
            var todoGroupResponses = _autoMapperProvider.Mapper.Map<List<TodoGroupResponse>>(todoGroupRecords);
            return todoGroupResponses;
        }

        public async Task<TodoGroupResponse> UpdateTodoGroup(Guid id, TodoGroupUpdateRequest todoGroupUpdateRequest)
        {
            var todoGroupRecord = await _todoGroupRepository.GetTodoGroup(id);
            todoGroupRecord = _autoMapperProvider.Mapper.Map(todoGroupUpdateRequest, todoGroupRecord);

            var googleId = _tokenHandlerProvider.GetSubFromToken();
            var userRecord = await _userRepository.GetUserByGoogleId(googleId);
            todoGroupRecord.LastModifiedBy = userRecord.Id;
            todoGroupRecord.LastModifiedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            todoGroupRecord = await _todoGroupRepository.UpdateTodoGroup(todoGroupRecord);
            var todoGroupResponse = _autoMapperProvider.Mapper.Map<TodoGroupResponse>(todoGroupRecord);
            return todoGroupResponse;
        }

        public async Task ShareTodoGroup(Guid Groupid, string email)
        {
            var user = await _userRepository.GetUserByEmailId(email);
            
            var userTodo = await _userTodoRepository.GetUserTodo(user.Id);
            userTodo.GroupIds.Add(Groupid);
            
            await _userTodoRepository.UpdateUserTodo(userTodo);
        }

        public async Task<bool> DeleteTodoGroup(Guid id)
        {
            return await _todoGroupRepository.DeleteTodoGroup(id);
        }
    }
}
