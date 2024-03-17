using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Services.Todo.Group;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace all_tech_webapp_service.Controllers.ToDo
{
    [Route("api/todoGroup")]
    [ApiController]
    public class TodoGroupController : Controller
    {
        private readonly ITodoGroupService _todoGroupService;
        private readonly TelemetryClient _telemetryClient;

        public TodoGroupController(ITodoGroupService todoGroupService, TelemetryClient telemetryClient)
        {
            _todoGroupService = todoGroupService ?? throw new ArgumentNullException(nameof(todoGroupService));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }
        
        /// <summary>
        /// Gets a To Do Group by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">To Do Group Found</response>
        /// <response code="404">If To Do Group was not found</response>
        /// <response code="500">If there was a problem getting Region Records</response>
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetTodoGroup([FromRoute] Guid id)
        {
            try
            {
                var todoGroupResponse = await _todoGroupService.GetTodoGroup(id);
                return Ok(todoGroupResponse);
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
        public async Task<IActionResult> CreateTodoGroup(TodoGroupCreateRequest todoGroupRequest)
        {
            try
            {
                var todoGroupResponse = await _todoGroupService.CreateTodoGroup(todoGroupRequest);
                return Ok(todoGroupResponse);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateTodoGroup([FromRoute] Guid id, TodoGroupUpdateRequest todoGroupUpdateRequest)
        {
            try
            {
                var todoGroupResponse = await _todoGroupService.UpdateTodoGroup(id, todoGroupUpdateRequest);
                return Ok(todoGroupResponse);
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
        public async Task<IActionResult> DeleteTodoGroup([FromRoute] Guid id)
        {
            try
            {
                var isDeleted = await _todoGroupService.DeleteTodoGroup(id);
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
