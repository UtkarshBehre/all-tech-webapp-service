using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Models.Todo.Item;

namespace all_tech_webapp_service.Models.Todo
{
    public class AllItemsResponse
    {
        public IEnumerable<TodoItemResponse> TodoItems { get; set; }

        public Dictionary<string, TodoGroupResponse> GroupsMap { get; set; }

        public AllItemsResponse()
        {
            TodoItems = new List<TodoItemResponse>();
            GroupsMap = new Dictionary<string, TodoGroupResponse>();
        }
    }
}
