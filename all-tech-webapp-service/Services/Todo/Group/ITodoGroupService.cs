using all_tech_webapp_service.Models.Todo.Group;

namespace all_tech_webapp_service.Services.Todo.Group
{
    public interface ITodoGroupService
    {
        /// <summary>
        /// Creates a new TodoGroup
        /// </summary>
        /// <param name="todoGroupCreateRequest">todo Group Create Request</param>
        /// <returns></returns>
        Task<TodoGroupResponse> CreateTodoGroup(TodoGroupCreateRequest todoGroupCreateRequest);

        /// <summary>
        /// Gets a TodoGroup by Id
        /// </summary>
        /// <param name="id">todo Group Id</param>
        /// <returns></returns>
        Task<TodoGroupResponse> GetTodoGroup(Guid id);

        /// <summary>
        /// Updates a TodoGroup
        /// </summary>
        /// <param name="id">todo Group Id</param>
        /// <param name="todoGroupUpdateRequest">todo Group Update Request</param>
        /// <returns></returns>
        Task<TodoGroupResponse> UpdateTodoGroup(Guid id, TodoGroupUpdateRequest todoGroupUpdateRequest);

        /// <summary>
        /// Deletes a TodoGroup by Id
        /// </summary>
        /// <param name="id">todo Group Id</param>
        /// <returns></returns>
        Task<bool> DeleteTodoGroup(Guid id);
    }
}
