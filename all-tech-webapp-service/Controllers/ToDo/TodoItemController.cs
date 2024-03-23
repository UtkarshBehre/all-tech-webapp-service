using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Services.Todo.Item;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace all_tech_webapp_service.Controllers.Todo
{
    [Route("api/todoItem")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService ?? throw new ArgumentNullException(nameof(todoItemService));
        }
        
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllTodoItems()
        {
            var todoItemResponses = await _todoItemService.GetAllTodoItems();
            return Ok(todoItemResponses);
        }

        [HttpGet]
        [Route("all/{groupId:guid}/{isComplete:bool}")]
        public async Task<IActionResult> GetAllTodoItemsByGroupId([FromRoute] Guid groupId, [FromRoute] bool isComplete)
        {
            var todoItemResponses = await _todoItemService.GetAllTodoItemsByGroupId(groupId, isComplete);
            return Ok(todoItemResponses);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetTodoItem([FromRoute] Guid id)
        {
            var todoItemResponse = await _todoItemService.GetTodoItem(id);
            return Ok(todoItemResponse);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTodoItem(TodoItemCreateRequest todoItemRequest)
        {
            var todoItemResponse = await _todoItemService.CreateTodoItem(todoItemRequest);
            return Ok(todoItemResponse);
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateTodoItem([FromRoute] Guid id, TodoItemUpdateRequest todoItemUpdateRequest)
        {
            var todoItemResponse = await _todoItemService.UpdateTodoItem(id, todoItemUpdateRequest);
            return Ok(todoItemResponse);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTodoItem([FromRoute] Guid id)
        {
            var isDeleted = await _todoItemService.DeleteTodoItem(id);
            return Ok(isDeleted);
        }
    }
}
