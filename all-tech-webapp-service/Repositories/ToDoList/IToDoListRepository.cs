using all_tech_webapp_service.Models.ToDoItem;

namespace all_tech_webapp_service.Repositories.ToDoList
{
    public interface IToDoListRepository
    {
        /// <summary>
        /// Creates new To Do Item
        /// </summary>
        /// <param name="toDoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        Task<ToDoItemRecord> CreateToDoItem(ToDoItemRecord toDoItemRecord);

        /// <summary>
        /// Gets all To Do Items
        /// </summary>
        /// <returns></returns>
        Task<List<ToDoItemRecord>> GetAllToDoItems();

        /// <summary>
        /// Gets a To Do Item by Id
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        Task<ToDoItemRecord> GetToDoItem(Guid id);

        /// <summary>
        /// Updates a To Do Item
        /// </summary>
        /// <param name="toDoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        Task<ToDoItemRecord> UpdateToDoItem(ToDoItemRecord toDoItemRecord);

        /// <summary>
        /// Deletes a To Do Item
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        Task<bool> DeleteToDoItem(Guid id);
    }
}