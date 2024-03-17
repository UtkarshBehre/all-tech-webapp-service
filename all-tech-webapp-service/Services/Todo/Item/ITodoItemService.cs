using all_tech_webapp_service.Models.Todo.Item;

namespace all_tech_webapp_service.Services.Todo.Item
{
    public interface ITodoItemService
    {
        /// <summary>
        /// Creates a new TodoItem
        /// </summary>
        /// <param name="todoItemCreateRequest">Todo Item Create Request</param>
        /// <returns></returns>
        Task<TodoItemResponse> CreateTodoItem(TodoItemCreateRequest todoItemCreateRequest);

        /// <summary>
        /// Gets all TodoItems
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TodoItemResponse>> GetAllTodoItems();

        /// <summary>
        /// Gets all TodoItems by Group Id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<IEnumerable<TodoItemResponse>> GetAllTodoItemsByGroupId(Guid groupId);

        /// <summary>
        /// Gets a TodoItem by Id
        /// </summary>
        /// <param name="id">Todo Item Id</param>
        /// <returns></returns>
        Task<TodoItemResponse> GetTodoItem(Guid id);

        /// <summary>
        /// Updates a TodoItem
        /// </summary>
        /// <param name="id">Todo Item Id</param>
        /// <param name="todoItemUpdateRequest">Todo Item Update Request</param>
        /// <returns></returns>
        Task<TodoItemResponse> UpdateTodoItem(Guid id, TodoItemUpdateRequest todoItemUpdateRequest);

        /// <summary>
        /// Deletes a TodoItem by Id
        /// </summary>
        /// <param name="id">Todo Item Id</param>
        /// <returns></returns>
        Task<bool> DeleteTodoItem(Guid id);
    }
}