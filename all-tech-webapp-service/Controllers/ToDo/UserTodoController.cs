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
            var userTodoResponses = await _userTodoService.GetAllUserTodos();
            return Ok(userTodoResponses);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserTodo([FromRoute] Guid id)
        {
            var userTodoResponse = await _userTodoService.GetUserTodo(id);
            return Ok(userTodoResponse);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUserTodo(UserTodoCreateRequest userTodoRequest)
        {
            var userTodoResponse = await _userTodoService.CreateUserTodo(userTodoRequest);
            return Ok(userTodoResponse);
        }

        [HttpPut]
        [Route("update/{id:guid}")]
        public async Task<IActionResult> UpdateUserTodo([FromRoute] Guid id, UserTodoUpdateRequest userTodoUpdateRequest)
        {
            var userTodoResponse = await _userTodoService.UpdateUserTodo(id, userTodoUpdateRequest);
            return Ok(userTodoResponse);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUserTodo([FromRoute] Guid id)
        {
            var isDeleted = await _userTodoService.DeleteUserTodo(id);
            return Ok(isDeleted);
        }
    }
}
