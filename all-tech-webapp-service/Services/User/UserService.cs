using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Models.Todo.UserTodo;
using all_tech_webapp_service.Models.User;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.User;
using all_tech_webapp_service.Services.Todo.Group;
using all_tech_webapp_service.Services.Todo.Item;
using all_tech_webapp_service.Services.Todo.UserTodo;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using UserResponse = all_tech_webapp_service.Models.User.UserResponse;

namespace all_tech_webapp_service.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly ITodoGroupService _todoGroupService;
        private readonly IUserTodoService _userTodoService;
        private readonly ITodoItemService _todoItemService;
        private readonly IAutoMapperProvider _autoMapperProvider;
        private readonly ITokenHandlerProvider _tokenHandlerProvider;
        private readonly TelemetryClient _telemetryClient;

        /// <summary>
        /// Contrustor for UserService
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="todoGroupService"></param>
        /// <param name="tokenHandlerProvider"></param>
        /// <param name="userTodoService"></param>
        /// <param name="telemetryClient"></param>
        /// <param name="autoMapperProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserService(IUserRepository userRepository, ITodoGroupService todoGroupService, IUserTodoService userTodoService, ITodoItemService todoItemService, ITokenHandlerProvider tokenHandlerProvider, IAutoMapperProvider autoMapperProvider, TelemetryClient telemetryClient)
        {
            _UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _todoGroupService = todoGroupService ?? throw new ArgumentNullException(nameof(todoGroupService));
            _userTodoService = userTodoService ?? throw new ArgumentNullException(nameof(userTodoService));
            _todoItemService = todoItemService ?? throw new ArgumentNullException(nameof(todoItemService));
            _tokenHandlerProvider = tokenHandlerProvider ?? throw new ArgumentNullException(nameof(tokenHandlerProvider));
            _autoMapperProvider = autoMapperProvider ?? throw new ArgumentNullException(nameof(autoMapperProvider));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public async Task<UserResponse> CreateUser()
        {
            var userCreateRequest = _tokenHandlerProvider.GetUserCreateRequestFromToken();
            var userRecord = _autoMapperProvider.Mapper.Map<UserRecord>(userCreateRequest);
            
            userRecord = await _UserRepository.CreateUser(userRecord);

            var todoGroup = await _todoGroupService.CreateTodoGroup(new TodoGroupCreateRequest
            {
                Name = "Default",
            });

            await _userTodoService.CreateUserTodo(new UserTodoCreateRequest
            {
                Id = userRecord.Id,
                GroupIds = new List<Guid> { todoGroup.Id }
            });

            await _todoItemService.CreateTodoItem(new TodoItemCreateRequest
            {
                GroupId = todoGroup.Id,
                Title = "Start Creating some todo tasks",
            });

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

        public async Task<UserResponse> GetUserByEmailId(string email)
        {
            var userRecord = await _UserRepository.GetUserByEmailId(email);
            var userResponse = _autoMapperProvider.Mapper.Map<UserResponse>(userRecord);
            return userResponse;
        }

        public async Task<UserResponse> GetUserByGoogleId(string googleId)
        {
            UserResponse userResponse = null;
                
            try
            {
                var userRecord = await _UserRepository.GetUserByGoogleId(googleId);
                userResponse = _autoMapperProvider.Mapper.Map<UserResponse>(userRecord);
            }
            catch (FileNotFoundException)
            {
                _telemetryClient.TrackTrace($"User not found with google Id: {googleId}. Hence creating a new one", SeverityLevel.Information);
            }
            
            if (userResponse == null)
            {
                userResponse = await CreateUser();
            }

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
