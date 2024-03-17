using all_tech_webapp_service.Models.Todo.Basic;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.UserTodo
{
    public class UserTodoRecord : BasicRecord
    {
        [JsonProperty("groupIds")]
        public List<Guid> GroupIds { get; set; }
        
        public UserTodoRecord() : base()
        {
            RecordType = RecordType.UserTodo;
        }
    }
}
