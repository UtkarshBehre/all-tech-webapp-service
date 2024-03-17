using all_tech_webapp_service.Models.Todo.Basic;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.Item
{
    public class TodoItemRecord: BasicRecord
    {
        [JsonProperty("groupId")]
        public Guid GroupId { get; set; }

        [JsonProperty("title")]
        public required string Title { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        public TodoItemRecord(): base()
        {
            IsComplete = false;
            RecordType = RecordType.TodoItem;
        }
    }
}
