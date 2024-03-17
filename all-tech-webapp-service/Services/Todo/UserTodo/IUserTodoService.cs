using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Models.Todo.UserTodo;

namespace all_tech_webapp_service.Services.Todo.UserTodo
{
    public interface IUserTodoService
    {
        /// <summary>
        /// Creates a new UserTodo
        /// </summary>
        /// <param name="userTodoCreateRequest">User Todo Create Request</param>
        /// <returns></returns>
        Task<UserTodoResponse> CreateUserTodo(UserTodoCreateRequest userTodoCreateRequest);

        /// <summary>
        /// Gets all UserTodos
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserTodoResponse>> GetAllUserTodos();

        /// <summary>
        /// Gets a UserTodo by Id
        /// </summary>
        /// <param name="id">User Todo Id</param>
        /// <returns></returns>
        Task<UserTodoResponse> GetUserTodo(Guid id);

        /// <summary>
        /// Updates a UserTodo
        /// </summary>
        /// <param name="id">User Todo Id</param>
        /// <param name="userTodoUpdateRequest">User Todo Update Request</param>
        /// <returns></returns>
        Task<UserTodoResponse> UpdateUserTodo(Guid id, UserTodoUpdateRequest userTodoUpdateRequest);

        /// <summary>
        /// Deletes a UserTodo by Id
        /// </summary>
        /// <param name="id">User Todo Id</param>
        /// <returns></returns>
        Task<bool> DeleteUserTodo(Guid id);
    }
}
