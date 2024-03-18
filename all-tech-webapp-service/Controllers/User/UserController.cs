using all_tech_webapp_service.Services.User;
using all_tech_webapp_service.Models.User;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace all_tech_webapp_service.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly TelemetryClient _telemetryClient;

        public UserController(IUserService userService, TelemetryClient telemetryClient)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            //Request.Headers.TryGetValue("Authorization", out var authToken);
            //Console.WriteLine(TokenHandleProvider.GetSubFromToken(authToken));
            //_telemetryClient.TrackTrace($"Authorization Header: {authToken}", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning);

            var userResponse = await _userService.GetUser(id);
            return Ok(userResponse);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser(UserCreateRequest userRequest)
        {
            var userResponse = await _userService.CreateUser(userRequest);
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
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var isDeleted = await _userService.DeleteUser(id);
            return Ok(isDeleted);
        }
    }
}
