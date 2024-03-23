using all_tech_webapp_service.Models.Todo.Item;
using System.Linq.Expressions;

namespace all_tech_webapp_service.Repositories.Todo.TodoItem
{
    public interface ITodoItemRepository
    {
        /// <summary>
        /// Creates new To Do Item
        /// </summary>
        /// <param name="todoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        Task<TodoItemRecord> CreateTodoItem(TodoItemRecord todoItemRecord);

        /// <summary>
        /// Gets all To Do Items
        /// </summary>
        /// <returns></returns>
        Task<List<TodoItemRecord>> GetAllTodoItems();

        /// <summary>
        /// Gets all To Do Items using the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TodoItemRecord>> GetAllTodoItems(Expression<Func<TodoItemRecord, bool>> predicate);

        /// <summary>
        /// Gets a To Do Item by Id
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        Task<TodoItemRecord> GetTodoItem(Guid id);

        /// <summary>
        /// Updates a To Do Item
        /// </summary>
        /// <param name="todoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        Task<TodoItemRecord> UpdateTodoItem(TodoItemRecord todoItemRecord);

        /// <summary>
        /// Deletes a To Do Item
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        Task<bool> DeleteTodoItem(Guid id);
    }
}