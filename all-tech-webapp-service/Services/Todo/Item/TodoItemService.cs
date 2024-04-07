using System.Linq.Expressions;
using all_tech_webapp_service.Models.Todo;
using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.Todo.TodoItem;
using all_tech_webapp_service.Repositories.Todo.UserTodo;

namespace all_tech_webapp_service.Services.Todo.Item
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IUserTodoRepository _userTodoRepository;
        private readonly IAutoMapperProvider _autoMapperProvider;

        public TodoItemService(ITodoItemRepository todoItemRepository, IUserTodoRepository userTodoRepository, IAutoMapperProvider autoMapperProvider)
        {
            _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
            _userTodoRepository = userTodoRepository ?? throw new ArgumentNullException(nameof(userTodoRepository));
            _autoMapperProvider = autoMapperProvider ?? throw new ArgumentNullException(nameof(autoMapperProvider));
        }

        public async Task<TodoItemResponse> CreateTodoItem(TodoItemCreateRequest todoItemCreateRequest)
        {
            var todoItemRecord = _autoMapperProvider.Mapper.Map<TodoItemRecord>(todoItemCreateRequest);
            todoItemRecord = await _todoItemRepository.CreateTodoItem(todoItemRecord);
            var todoItemResponse = _autoMapperProvider.Mapper.Map<TodoItemResponse>(todoItemRecord);
            return todoItemResponse;
        }

        public async Task<IEnumerable<TodoItemResponse>> GetAllTodoItems()
        {
            var todoItemRecords = await _todoItemRepository.GetAllTodoItems();
            var todoItemResponses = _autoMapperProvider.Mapper.Map<IEnumerable<TodoItemResponse>>(todoItemRecords);
            return todoItemResponses;
        }

        public async Task<IEnumerable<TodoItemResponse>> GetAllTodoItemsByUser(Guid userId)
        {
            var userTodo = await _userTodoRepository.GetUserTodo(userId);
            var groupIds = userTodo.GroupIds;

            var todoItemResponses = await GetAllTodoItems(groupIds);
            return todoItemResponses;
        }

        public async Task<IEnumerable<TodoItemResponse>> GetAllTodoItems(List<Guid> groupIds)
        {
            Expression<Func<TodoItemRecord, bool>> predicate = x
                => groupIds.Contains(x.GroupId) &&
                   x.RecordType == RecordType.TodoItem &&
                   !x.IsDeleted;

            var todoItemRecords = await _todoItemRepository.GetAllTodoItems(predicate);
            var todoItemResponses = _autoMapperProvider.Mapper.Map<IEnumerable<TodoItemResponse>>(todoItemRecords);
            return todoItemResponses;
        }

        public async Task<IEnumerable<TodoItemResponse>> GetAllTodoItemsByGroupId(Guid groupId)
        {
            Expression<Func<TodoItemRecord, bool>> predicate = x
                => x.GroupId == groupId &&
                   x.RecordType == RecordType.TodoItem &&
                   !x.IsDeleted;

            var todoItemRecords = await _todoItemRepository.GetAllTodoItems(predicate);
            var todoItemResponses = _autoMapperProvider.Mapper.Map<IEnumerable<TodoItemResponse>>(todoItemRecords);
            return todoItemResponses;
        }

        public async Task<IEnumerable<TodoItemResponse>> GetAllTodoItemsByGroupId(Guid groupId, bool isComplete)
        {
            Expression<Func<TodoItemRecord, bool>> predicate = x
                => x.GroupId == groupId &&
                   x.IsComplete == isComplete &&
                   x.RecordType == RecordType.TodoItem &&
                   !x.IsDeleted;

            var todoItemRecords = await _todoItemRepository.GetAllTodoItems(predicate);
            var todoItemResponses = _autoMapperProvider.Mapper.Map<IEnumerable<TodoItemResponse>>(todoItemRecords);
            return todoItemResponses;
        }

        public async Task<TodoItemResponse> GetTodoItem(Guid id)
        {
            var todoItemRecord = await _todoItemRepository.GetTodoItem(id);
            var todoItemResponse = _autoMapperProvider.Mapper.Map<TodoItemResponse>(todoItemRecord);
            return todoItemResponse;
        }

        public async Task<TodoItemResponse> UpdateTodoItem(Guid id, TodoItemUpdateRequest todoItemUpdateRequest)
        {
            var todoItemRecord = await _todoItemRepository.GetTodoItem(id);
            todoItemRecord = _autoMapperProvider.Mapper.Map(todoItemUpdateRequest, todoItemRecord);
            todoItemRecord = await _todoItemRepository.UpdateTodoItem(todoItemRecord);
            var todoItemResponse = _autoMapperProvider.Mapper.Map<TodoItemResponse>(todoItemRecord);

            return todoItemResponse;
        }

        public async Task<bool> DeleteTodoItem(Guid id)
        {
            return await _todoItemRepository.DeleteTodoItem(id);
        }
    }
}
