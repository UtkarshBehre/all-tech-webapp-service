﻿using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Models.Todo.UserTodo;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Providers.Todo;
using all_tech_webapp_service.Repositories.Todo.TodoGroupRepository;
using all_tech_webapp_service.Services.Todo.UserTodo;
using all_tech_webapp_service.Services.User;

namespace all_tech_webapp_service.Services.Todo.Group
{
    public class TodoGroupService : ITodoGroupService
    {
        private readonly ITodoGroupRepository _todoGroupRepository;
        private readonly IUserService _userService;
        private readonly IUserTodoService _userTodoService;
        private readonly ITokenHandlerProvider _tokenHandlerProvider;
        private readonly ITodoHub _todoHub;
        private readonly IAutoMapperProvider _autoMapperProvider;

        /// <summary>
        /// Contrustor for TodoGroupService
        /// </summary>
        /// <param name="todoGroupRepository"></param>
        /// <param name="autoMapperProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TodoGroupService(ITodoGroupRepository todoGroupRepository, IUserService userService, IUserTodoService userTodoService, ITokenHandlerProvider tokenHandlerProvider, ITodoHub todoHub, IAutoMapperProvider autoMapperProvider)
        {
            _todoGroupRepository = todoGroupRepository ?? throw new ArgumentNullException(nameof(todoGroupRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userTodoService = userTodoService ?? throw new ArgumentNullException(nameof(userTodoService));
            _tokenHandlerProvider = tokenHandlerProvider ?? throw new ArgumentNullException(nameof(tokenHandlerProvider));
            _todoHub = todoHub ?? throw new ArgumentNullException(nameof(todoHub));
            _autoMapperProvider = autoMapperProvider ?? throw new ArgumentNullException(nameof(autoMapperProvider));
        }

        public async Task<TodoGroupResponse> CreateTodoGroup(TodoGroupCreateRequest todoGroupCreateRequest)
        {
            var todoGroupRecord = _autoMapperProvider.Mapper.Map<TodoGroupRecord>(todoGroupCreateRequest);
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

            todoGroupRecord = await _todoGroupRepository.UpdateTodoGroup(todoGroupRecord);
            var todoGroupResponse = _autoMapperProvider.Mapper.Map<TodoGroupResponse>(todoGroupRecord);
            return todoGroupResponse;
        }

        public async Task ShareTodoGroup(Guid Groupid, string email)
        {
            var user = await _userService.GetUserByEmailId(email);
            
            var userTodo = await _userTodoService.GetUserTodo(user.Id);
            userTodo.GroupIds.Add(Groupid);
            var userTodoRequest = _autoMapperProvider.Mapper.Map<UserTodoUpdateRequest>(userTodo);
            
            await _userTodoService.UpdateUserTodo(userTodo.Id, userTodoRequest);

            var googleId = _tokenHandlerProvider.GetSubFromToken();
            var currentUser = await _userService.GetUserByGoogleId(googleId);
            var todoGroup = await GetTodoGroup(Groupid);
            await _todoHub.SendTodoGroupSharedMessage($"{currentUser.FirstName} {currentUser.LastName}", todoGroup.Name, user.Id.ToString());
        }

        public async Task<bool> DeleteTodoGroup(Guid id)
        {
            return await _todoGroupRepository.DeleteTodoGroup(id);
        }
    }
}
