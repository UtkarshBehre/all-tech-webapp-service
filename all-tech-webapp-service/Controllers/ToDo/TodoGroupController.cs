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

        public TodoGroupController(ITodoGroupService todoGroupService)
        {
            _todoGroupService = todoGroupService ?? throw new ArgumentNullException(nameof(todoGroupService));
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetTodoGroup([FromRoute] Guid id)
        {
            var todoGroupResponse = await _todoGroupService.GetTodoGroup(id);
            return Ok(todoGroupResponse);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTodoGroup(TodoGroupCreateRequest todoGroupRequest)
        {
            var todoGroupResponse = await _todoGroupService.CreateTodoGroup(todoGroupRequest);
            return Ok(todoGroupResponse);
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateTodoGroup([FromRoute] Guid id, TodoGroupUpdateRequest todoGroupUpdateRequest)
        {
            var todoGroupResponse = await _todoGroupService.UpdateTodoGroup(id, todoGroupUpdateRequest);
            return Ok(todoGroupResponse);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTodoGroup([FromRoute] Guid id)
        {
            var isDeleted = await _todoGroupService.DeleteTodoGroup(id);
            return Ok(isDeleted);
        }
    }
}
