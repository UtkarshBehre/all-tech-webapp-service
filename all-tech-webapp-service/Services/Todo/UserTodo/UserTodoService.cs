using all_tech_webapp_service.Repositories.Todo.UserTodo;
using all_tech_webapp_service.Models.Todo.UserTodo;
using all_tech_webapp_service.Providers.Mapper;
using all_tech_webapp_service.Providers.Token;
using all_tech_webapp_service.Repositories.User;

namespace all_tech_webapp_service.Services.Todo.UserTodo
{
    public class UserTodoService : IUserTodoService
    {
        private readonly IUserTodoRepository _UserTodoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandlerProvider _tokenHandlerProvider;
        private readonly IAutoMapperProvider _autoMapperProvider;

        /// <summary>
        /// Contrustor for UserTodoService
        /// </summary>
        /// <param name="userTodoRepository"></param>
        /// <param name="autoMapperProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserTodoService(IUserTodoRepository userTodoRepository, IUserRepository userRepository, ITokenHandlerProvider tokenHandlerProvider, IAutoMapperProvider autoMapperProvider)
        {
            _UserTodoRepository = userTodoRepository ?? throw new ArgumentNullException(nameof(userTodoRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenHandlerProvider = tokenHandlerProvider ?? throw new ArgumentNullException(nameof(tokenHandlerProvider));
            _autoMapperProvider = autoMapperProvider ?? throw new ArgumentNullException(nameof(autoMapperProvider));
        }

        public async Task<UserTodoResponse> CreateUserTodo(UserTodoCreateRequest userTodoCreateRequest)
        {
            var userTodoRecord = _autoMapperProvider.Mapper.Map<UserTodoRecord>(userTodoCreateRequest);
            var googleId = _tokenHandlerProvider.GetSubFromToken();
            var userRecord = await _userRepository.GetUserByGoogleId(googleId);
            userTodoRecord.CreatedBy = userRecord.Id;
            userTodoRecord.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            userTodoRecord = await _UserTodoRepository.CreateUserTodo(userTodoRecord);
            var userTodoResponse = _autoMapperProvider.Mapper.Map<UserTodoResponse>(userTodoRecord);
            return userTodoResponse;
        }

        public async Task<IEnumerable<UserTodoResponse>> GetAllUserTodos()
        {
            var userTodoRecords = await _UserTodoRepository.GetAllUserTodos();
            var userTodoResponses = _autoMapperProvider.Mapper.Map<IEnumerable<UserTodoResponse>>(userTodoRecords);
            return userTodoResponses;
        }

        public async Task<UserTodoResponse> GetUserTodo(Guid id)
        {
            var userTodoRecord = await _UserTodoRepository.GetUserTodo(id);
            var userTodoResponse = _autoMapperProvider.Mapper.Map<UserTodoResponse>(userTodoRecord);
            return userTodoResponse;
        }

        public async Task<UserTodoResponse> UpdateUserTodo(Guid id, UserTodoUpdateRequest userTodoUpdateRequest)
        {
            var userTodoRecord = await _UserTodoRepository.GetUserTodo(id);
            userTodoRecord = _autoMapperProvider.Mapper.Map(userTodoUpdateRequest, userTodoRecord);
            
            var googleId = _tokenHandlerProvider.GetSubFromToken();
            var userRecord = await _userRepository.GetUserByGoogleId(googleId);
            userTodoRecord.LastModifiedBy = userRecord.Id;
            userTodoRecord.LastModifiedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            userTodoRecord = await _UserTodoRepository.UpdateUserTodo(userTodoRecord);
            var userTodoResponse = _autoMapperProvider.Mapper.Map<UserTodoResponse>(userTodoRecord);
            return userTodoResponse;
        }

        public async Task<bool> DeleteUserTodo(Guid id)
        {
            return await _UserTodoRepository.DeleteUserTodo(id);
        }
    }
}
