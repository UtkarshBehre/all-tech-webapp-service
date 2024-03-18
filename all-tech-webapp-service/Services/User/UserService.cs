using all_tech_webapp_service.Models.User;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.User;
using Microsoft.Azure.Cosmos;
using UserResponse = all_tech_webapp_service.Models.User.UserResponse;

namespace all_tech_webapp_service.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IAutoMapperProvider _autoMapperProvider;
        private readonly ITokenHandlerProvider _tokenHandlerProvider;

        /// <summary>
        /// Contrustor for UserService
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="autoMapperProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserService(IUserRepository userRepository, ITokenHandlerProvider tokenHandlerProvider, IAutoMapperProvider autoMapperProvider)
        {
            _UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenHandlerProvider = tokenHandlerProvider ?? throw new ArgumentNullException(nameof(tokenHandlerProvider));
            _autoMapperProvider = autoMapperProvider ?? throw new ArgumentNullException(nameof(autoMapperProvider));
        }

        public async Task<UserResponse> CreateUser(UserCreateRequest userCreateRequest)
        {
            var userRecord = _autoMapperProvider.Mapper.Map<UserRecord>(userCreateRequest);
            userRecord = await _UserRepository.CreateUser(userRecord);
            var userResponse = _autoMapperProvider.Mapper.Map<UserResponse>(userRecord);
            return userResponse;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var userRecords = await _UserRepository.GetAllUsers();
            var userResponses = _autoMapperProvider.Mapper.Map<IEnumerable<UserResponse>>(userRecords);
            return userResponses;
        }

        public async Task<UserResponse> GetUser(Guid id)
        {
            var userRecord = await _UserRepository.GetUser(id);
            var userResponse = _autoMapperProvider.Mapper.Map<UserResponse>(userRecord);
            return userResponse;
        }

        public async Task<UserResponse> GetUserByGoogleId(string googleId)
        {
            UserRecord userRecord;
                
            try
            {
                userRecord = await _UserRepository.GetUserByGoogleId(googleId);
            }
            catch (FileNotFoundException)
            {
                var userCreateRequest = _tokenHandlerProvider.GetUserCreateRequestFromToken();
                userRecord = _autoMapperProvider.Mapper.Map<UserRecord>(userCreateRequest);
                userRecord = await _UserRepository.CreateUser(userRecord);
            }
            
            var userResponse = _autoMapperProvider.Mapper.Map<UserResponse>(userRecord);
            return userResponse;
        }

        public async Task<UserResponse> UpdateUser(Guid id, UserUpdateRequest userUpdateRequest)
        {
            var userRecord = await _UserRepository.GetUser(id);
            userRecord = _autoMapperProvider.Mapper.Map(userUpdateRequest, userRecord);

            userRecord = await _UserRepository.UpdateUser(userRecord);
            var userResponse = _autoMapperProvider.Mapper.Map<UserResponse>(userRecord);
            return userResponse;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            return await _UserRepository.DeleteUser(id);
        }
    }
}
