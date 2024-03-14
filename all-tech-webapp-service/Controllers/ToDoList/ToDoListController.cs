﻿using all_tech_webapp_service.Models.ToDoItem;
using all_tech_webapp_service.Services.ToDoList;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace all_tech_webapp_service.Controllers.ToDoList
{
    [Route("api/todo")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly IToDoListService _toDoListService;
        private readonly TelemetryClient _telemetryClient;

        public ToDoListController(IToDoListService toDoListService, TelemetryClient telemetryClient)
        {
            _toDoListService = toDoListService ?? throw new ArgumentNullException(nameof(toDoListService));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllToDoListItems()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out var authHeader);
                _telemetryClient.TrackTrace($"Authorization Header: {authHeader}", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning);
                var toDoItemResponses = await _toDoListService.GetAllToDoItems();
                return Ok(toDoItemResponses);
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
        [Route("items/{id:guid}")]
        public async Task<IActionResult> GetToDoItem([FromRoute] Guid id)
        {
            try
            {
                var toDoItemResponse = await _toDoListService.GetToDoItem(id);
                return Ok(toDoItemResponse);
            }
            catch(CosmosException ex)
            {
                _telemetryClient.TrackTrace(ex.Message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
                return NotFound(ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                var error = $"{ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
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
        public async Task<IActionResult> CreateToDoItem(ToDoItemCreateRequest toDoItemRequest)
        {
            try
            {
                var toDoItemResponse = await _toDoListService.CreateToDoItem(toDoItemRequest);
                return Ok(toDoItemResponse);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateToDoItem(ToDoItemUpdateRequest toDoItemUpdateRequest)
        {
            try
            {
                var toDoItemResponse = await _toDoListService.UpdateToDoItem(toDoItemUpdateRequest);
                return Ok(toDoItemResponse);
            }
            catch (FileNotFoundException ex)
            {
                var error = $"{ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
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
        public async Task<IActionResult> DeleteToDoItem([FromRoute] Guid id)
        {
            try
            {
                var isDeleted = await _toDoListService.DeleteToDoItem(id);
                return Ok(isDeleted);
            }
            catch (FileNotFoundException ex)
            {
                var error = $"{ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);
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
