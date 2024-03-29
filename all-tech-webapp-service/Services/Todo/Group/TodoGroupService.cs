﻿using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.Todo.TodoGroupRepository;

namespace all_tech_webapp_service.Services.Todo.Group
{
    public class TodoGroupService : ITodoGroupService
    {
        private readonly ITodoGroupRepository _todoGroupRepository;
        private readonly IAutoMapperProvider _autoMapperProvider;

        /// <summary>
        /// Contrustor for TodoGroupService
        /// </summary>
        /// <param name="todoGroupRepository"></param>
        /// <param name="autoMapperProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TodoGroupService(ITodoGroupRepository todoGroupRepository, IAutoMapperProvider autoMapperProvider)
        {
            _todoGroupRepository = todoGroupRepository ?? throw new ArgumentNullException(nameof(todoGroupRepository));
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

        public async Task<bool> DeleteTodoGroup(Guid id)
        {
            return await _todoGroupRepository.DeleteTodoGroup(id);
        }
    }
}
