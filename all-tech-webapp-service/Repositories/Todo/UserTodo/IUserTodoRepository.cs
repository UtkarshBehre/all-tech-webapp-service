using all_tech_webapp_service.Models.Todo.UserTodo;

namespace all_tech_webapp_service.Repositories.Todo.UserTodo
{
    public interface IUserTodoRepository
    {
        /// <summary>
        /// Creates new User Todo
        /// </summary>
        /// <param name="userTodo">User Todo Record</param>
        /// <returns></returns>
        Task<UserTodoRecord> CreateUserTodo(UserTodoRecord userTodo);

        /// <summary>
        /// Gets all User Todo
        /// </summary>
        /// <returns></returns>
        Task<List<UserTodoRecord>> GetAllUserTodos();

        /// <summary>
        /// Gets a User Todo by Id
        /// </summary>
        /// <param name="id">User Todo Id</param>
        /// <returns></returns>
        Task<UserTodoRecord> GetUserTodo(Guid id);

        /// <summary>
        /// Updates a User Todo
        /// </summary>
        /// <param name="userTodo">User Todo Record</param>
        /// <returns></returns>
        Task<UserTodoRecord> UpdateUserTodo(UserTodoRecord userTodo);

        /// <summary>
        /// Deletes a User Todo
        /// </summary>
        /// <param name="id">User Todo Id</param>
        /// <returns></returns>
        Task<bool> DeleteUserTodo(Guid id);
    }
}
