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
        /// Gets all TodoGroups by group Ids
        /// </summary>
        /// <param name="ids">List of Group Ids</param>
        /// <returns></returns>
        Task<List<TodoGroupResponse>> GetTodoGroups(List<Guid> ids);

        /// <summary>
        /// Updates a TodoGroup
        /// </summary>
        /// <param name="id">todo Group Id</param>
        /// <param name="todoGroupUpdateRequest">todo Group Update Request</param>
        /// <returns></returns>
        Task<TodoGroupResponse> UpdateTodoGroup(Guid id, TodoGroupUpdateRequest todoGroupUpdateRequest);

        /// <summary>
        /// Share a TodoGroup with a user
        /// </summary>
        /// <param name="Groupid">Todo Group Id</param>
        /// <param name="email">Email</param>
        /// <returns></returns>
        Task ShareTodoGroup(Guid Groupid, string email);

        /// <summary>
        /// Deletes a TodoGroup by Id
        /// </summary>
        /// <param name="id">todo Group Id</param>
        /// <returns></returns>
        Task<bool> DeleteTodoGroup(Guid id);
    }
}
