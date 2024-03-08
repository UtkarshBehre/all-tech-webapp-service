using all_tech_webapp_service.Models.ToDoItem;

namespace all_tech_webapp_service.Services.ToDoList
{
    public interface IToDoListService
    {
        /// <summary>
        /// Creates a new ToDoItem
        /// </summary>
        /// <param name="toDoItemCreateRequest">ToDo Item Create Request</param>
        /// <returns></returns>
        Task<ToDoItemResponse> CreateToDoItem(ToDoItemCreateRequest toDoItemCreateRequest);

        /// <summary>
        /// Gets all ToDoItems
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ToDoItemResponse>> GetAllToDoItems();

        /// <summary>
        /// Gets a ToDoItem by Id
        /// </summary>
        /// <param name="id">ToDo Item Id</param>
        /// <returns></returns>
        Task<ToDoItemResponse> GetToDoItem(Guid id);

        /// <summary>
        /// Updates a ToDoItem
        /// </summary>
        /// <param name="toDoItemUpdateRequest">ToDo Item Update Request</param>
        /// <returns></returns>
        Task<ToDoItemResponse> UpdateToDoItem(ToDoItemUpdateRequest toDoItemUpdateRequest);

        /// <summary>
        /// Deletes a ToDoItem by Id
        /// </summary>
        /// <param name="id">ToDo Item Id</param>
        /// <returns></returns>
        Task<bool> DeleteToDoItem(Guid id);
    }
}