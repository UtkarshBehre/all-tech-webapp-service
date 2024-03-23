using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.Todo.TodoItem;

namespace all_tech_webapp_service.Services.Todo.Item
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IAutoMapperProvider _autoMapperProvider;

        public TodoItemService(ITodoItemRepository todoItemRepository, IAutoMapperProvider autoMapperProvider)
        {
            _todoItemRepository = todoItemRepository ?? throw new ArgumentNullException(nameof(todoItemRepository));
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

        public async Task<IEnumerable<TodoItemResponse>> GetAllTodoItemsByGroupId(Guid groupId, bool isComplete)
        {
            var todoItemRecords = await _todoItemRepository.GetAllTodoItemByGroupId(groupId, isComplete);
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
