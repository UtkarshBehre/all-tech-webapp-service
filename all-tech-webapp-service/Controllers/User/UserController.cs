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
            try
            {
                var userResponse = await _userService.GetUser(id);
                return Ok(userResponse);
            }
            catch (CosmosException ex)
            {
                _telemetryClient.TrackTrace(ex.Message, SeverityLevel.Information);
                return NotFound(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                var error = $"{ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, SeverityLevel.Information);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser(UserCreateRequest userRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                    

                var userResponse = await _userService.CreateUser(userRequest);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, UserUpdateRequest userUpdateRequest)
        {
            try
            {
                var userResponse = await _userService.UpdateUser(id, userUpdateRequest);
                return Ok(userResponse);
            }
            catch (FileNotFoundException ex)
            {
                var error = $"{ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, SeverityLevel.Information);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            try
            {
                var isDeleted = await _userService.DeleteUser(id);
                return Ok(isDeleted);
            }
            catch (FileNotFoundException ex)
            {
                var error = $"{ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, SeverityLevel.Information);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
