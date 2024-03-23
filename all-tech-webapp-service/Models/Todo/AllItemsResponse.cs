using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Models.Todo.Item;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo
{
    public class AllItemsResponse
    {
        [JsonProperty("todoItems")]
        public IEnumerable<TodoItemResponse> TodoItems { get; set; }

        [JsonProperty("groupsMap")]
        public IEnumerable<TodoGroupResponse> GroupsMap { get; set; }

        public AllItemsResponse()
        {
            TodoItems = new List<TodoItemResponse>();
            GroupsMap = new List<TodoGroupResponse>();
        }
    }
}
