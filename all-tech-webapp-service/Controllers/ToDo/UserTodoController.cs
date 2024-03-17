using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Services.Todo.Item;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using all_tech_webapp_service.Services.Todo.UserTodo;
using all_tech_webapp_service.Models.Todo.UserTodo;

namespace all_tech_webapp_service.Controllers.ToDo
{
    [Route("api/userTodo")]
    [ApiController]
    public class UserTodoController : Controller
    {
        private readonly IUserTodoService _userTodoService;
        private readonly TelemetryClient _telemetryClient;

        public UserTodoController(IUserTodoService userTodoService, TelemetryClient telemetryClient)
        {
            _userTodoService = userTodoService ?? throw new ArgumentNullException(nameof(userTodoService));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllUserTodos()
        {
            try
            {
                var userTodoResponses = await _userTodoService.GetAllUserTodos();
                return Ok(userTodoResponses);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserTodo([FromRoute] Guid id)
        {
            try
            {
                var userTodoResponse = await _userTodoService.GetUserTodo(id);
                return Ok(userTodoResponse);
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
        public async Task<IActionResult> CreateUserTodo(UserTodoCreateRequest userTodoRequest)
        {
            try
            {
                var userTodoResponse = await _userTodoService.CreateUserTodo(userTodoRequest);
                return Ok(userTodoResponse);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateUserTodo([FromRoute] Guid id, UserTodoUpdateRequest userTodoUpdateRequest)
        {
            try
            {
                var userTodoResponse = await _userTodoService.UpdateUserTodo(id, userTodoUpdateRequest);
                return Ok(userTodoResponse);
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
        public async Task<IActionResult> DeleteUserTodo([FromRoute] Guid id)
        {
            try
            {
                var isDeleted = await _userTodoService.DeleteUserTodo(id);
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
