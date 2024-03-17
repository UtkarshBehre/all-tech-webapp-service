using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Services.Todo.Item;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace all_tech_webapp_service.Controllers.Todo
{
    [Route("api/todoItem")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly TelemetryClient _telemetryClient;

        public TodoItemController(ITodoItemService todoItemService, TelemetryClient telemetryClient)
        {
            _todoItemService = todoItemService ?? throw new ArgumentNullException(nameof(todoItemService));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllTodoItems()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                _telemetryClient.TrackTrace($"Authorization Header: {authHeader}", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning);
                var todoItemResponses = await _todoItemService.GetAllTodoItems();
                return Ok(todoItemResponses);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("all/{groupId:guid}")]
        public async Task<IActionResult> GetAllTodoItemsByGroupId([FromRoute] Guid groupId)
        {
            try
            {
                var todoItemResponses = await _todoItemService.GetAllTodoItemsByGroupId(groupId);
                return Ok(todoItemResponses);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Gets a To Do Item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">To Do Item Found</response>
        /// <response code="404">If To Do Item was not found</response>
        /// <response code="500">If there was a problem getting Region Records</response>
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetTodoItem([FromRoute] Guid id)
        {
            try
            {
                var todoItemResponse = await _todoItemService.GetTodoItem(id);
                return Ok(todoItemResponse);
            }
            catch(CosmosException ex)
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
        public async Task<IActionResult> CreateTodoItem(TodoItemCreateRequest todoItemRequest)
        {
            try
            {
                var todoItemResponse = await _todoItemService.CreateTodoItem(todoItemRequest);
                return Ok(todoItemResponse);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateTodoItem([FromRoute] Guid id, TodoItemUpdateRequest todoItemUpdateRequest)
        {
            try
            {
                var todoItemResponse = await _todoItemService.UpdateTodoItem(id, todoItemUpdateRequest);
                return Ok(todoItemResponse);
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
        public async Task<IActionResult> DeleteTodoItem([FromRoute] Guid id)
        {
            try
            {
                var isDeleted = await _todoItemService.DeleteTodoItem(id);
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
