using all_tech_webapp_service.Models.User;

namespace all_tech_webapp_service.Services.User
{
    public interface IUserService
    {
        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="userCreateRequest">User  Create Request</param>
        /// <returns></returns>
        Task<UserResponse> CreateUser();

        /// <summary>
        /// Gets all Users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserResponse>> GetAllUsers();

        /// <summary>
        /// Gets a User by Id
        /// </summary>
        /// <param name="id">User  Id</param>
        /// <returns></returns>
        Task<UserResponse> GetUser(Guid id);

        /// <summary>
        /// Gets a User by Google Id
        /// </summary>
        /// <param name="googleId">google Id</param>
        /// <returns></returns>
        Task<UserResponse> GetUserByGoogleId(string googleId);

        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="userUpdateRequest">User  Update Request</param>
        /// <returns></returns>
        Task<UserResponse> UpdateUser(Guid id, UserUpdateRequest userUpdateRequest);

        /// <summary>
        /// Deletes a User by Id
        /// </summary>
        /// <param name="id">User  Id</param>
        /// <returns></returns>
        Task<bool> DeleteUser(Guid id);
    }
}
