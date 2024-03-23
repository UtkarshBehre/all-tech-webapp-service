using all_tech_webapp_service.Models.Todo;
using all_tech_webapp_service.Services.Todo.Group;
using all_tech_webapp_service.Services.Todo.Item;
using all_tech_webapp_service.Services.Todo.UserTodo;

namespace all_tech_webapp_service.Services.Todo.DashBoard
{
    public class DashboardService : IDashboardService
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IUserTodoService _userTodoService;
        private readonly ITodoGroupService _todoGroupService;

        public DashboardService(ITodoItemService todoItemService, IUserTodoService userTodoService, ITodoGroupService todoGroupService)
        {
            _todoItemService = todoItemService ?? throw new ArgumentNullException(nameof(todoItemService));
            _userTodoService = userTodoService ?? throw new ArgumentNullException(nameof(userTodoService));
            _todoGroupService = todoGroupService ?? throw new ArgumentNullException(nameof(todoGroupService));
        }

        public async Task<AllItemsResponse> GetUserDashBoardData(Guid userId)
        {
            var userTodoRecord = await _userTodoService.GetUserTodo(userId);
            var todoItems = await _todoItemService.GetAllTodoItemsByUser(userId);
            var todoGroups = await _todoGroupService.GetTodoGroups(userTodoRecord.GroupIds);

            var allItemsResponse = new AllItemsResponse
            {
                TodoItems = todoItems,
                GroupsMap = todoGroups.ToDictionary(x => x.Id.ToString(), x => x)
            };

            return allItemsResponse;
        }
    }
}
