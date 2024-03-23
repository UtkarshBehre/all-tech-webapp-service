using all_tech_webapp_service.Models.Todo.Group;

namespace all_tech_webapp_service.Repositories.Todo.TodoGroupRepository
{
    public interface ITodoGroupRepository
    {
        /// <summary>
        /// Creates new To Do Group
        /// </summary>
        /// <param name="todoGroupRecord">To Do Group Record</param>
        /// <returns></returns>
        Task<TodoGroupRecord> CreateTodoGroup(TodoGroupRecord todoGroupRecord);

        /// <summary>
        /// Gets a To Do Group by Id
        /// </summary>
        /// <param name="id">To Do Group Id</param>
        /// <returns></returns>
        Task<TodoGroupRecord> GetTodoGroup(Guid id);

        /// <summary>
        /// Gets a To Do Groups by Ids
        /// </summary>
        /// <param name="ids">To Do Group Ids</param>
        /// <returns></returns>
        Task<List<TodoGroupRecord>> GetTodoGroups(List<Guid> ids);

        /// <summary>
        /// Updates a To Do Group
        /// </summary>
        /// <param name="todoGroupRecord">To Do Group Record</param>
        /// <returns></returns>
        Task<TodoGroupRecord> UpdateTodoGroup(TodoGroupRecord todoGroupRecord);

        /// <summary>
        /// Deletes a To Do Group
        /// </summary>
        /// <param name="id">To Do Group Id</param>
        /// <returns></returns>
        Task<bool> DeleteTodoGroup(Guid id);
    }
}
