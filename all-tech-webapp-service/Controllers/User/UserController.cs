using all_tech_webapp_service.Services.User;
using all_tech_webapp_service.Models.User;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using all_tech_webapp_service.Providers;

namespace all_tech_webapp_service.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly TelemetryClient _telemetryClient;
        private readonly ITokenHandlerProvider _tokenHandlerProvider;

        public UserController(IUserService userService, ITokenHandlerProvider tokenHandlerProvider, TelemetryClient telemetryClient)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _tokenHandlerProvider = tokenHandlerProvider ?? throw new ArgumentNullException(nameof(tokenHandlerProvider));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUser()
        {
            var googleId = _tokenHandlerProvider.GetSubFromToken();
            var userResponse = await _userService.GetUserByGoogleId(googleId);
            return Ok(userResponse);
        }

        [HttpGet]
        [Route("email/{emailId}")]
        public async Task<IActionResult> GetUserByEmailId([FromRoute] string emailId)
        {
            var userResponse = await _userService.GetUserByEmailId(emailId);
            return Ok(userResponse);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser()
        {
            var userResponse = await _userService.CreateUser();
            return Ok(userResponse);
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, UserUpdateRequest userUpdateRequest)
        {
            var userResponse = await _userService.UpdateUser(id, userUpdateRequest);
            return Ok(userResponse);
        }

        [HttpDelete]
        [Route("delete/{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var isDeleted = await _userService.DeleteUser(id);
            return Ok(isDeleted);
        }
    }
}
