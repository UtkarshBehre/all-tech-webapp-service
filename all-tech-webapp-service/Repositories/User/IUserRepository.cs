using all_tech_webapp_service.Models.User;

namespace all_tech_webapp_service.Repositories.User
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates new User 
        /// </summary>
        /// <param name="user">User Record</param>
        /// <returns></returns>
        Task<UserRecord> CreateUser(UserRecord user);

        /// <summary>
        /// Gets all User 
        /// </summary>
        /// <returns></returns>
        Task<List<UserRecord>> GetAllUsers();

        /// <summary>
        /// Gets a User by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        Task<UserRecord> GetUser(Guid id);

        /// <summary>
        /// Gets a User by Google Id
        /// </summary>
        /// <param name="googleId">Google Id</param>
        /// <returns></returns>
        Task<UserRecord> GetUserByGoogleId(string googleId);

        /// <summary>
        /// Updates a User 
        /// </summary>
        /// <param name="user">User Record</param>
        /// <returns></returns>
        Task<UserRecord> UpdateUser(UserRecord user);

        /// <summary>
        /// Deletes a User 
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid id);
    }
}
