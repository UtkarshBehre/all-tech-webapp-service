using all_tech_webapp_service.Models.Todo.Basic;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.UserTodo
{
    public class UserTodoResponse : BasicResponse
    {
        [JsonProperty("groupIds")]
        public List<Guid> GroupIds { get; set; }
    }
}
