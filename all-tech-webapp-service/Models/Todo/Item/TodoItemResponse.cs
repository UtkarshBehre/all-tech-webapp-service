using all_tech_webapp_service.Models.Todo.Basic;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.Item
{
    public class TodoItemResponse : BasicResponse
    {
        [JsonProperty("groupId")]
        public Guid GroupId { get; set; }

        [JsonProperty("title")]
        public required string Title { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        [JsonProperty("completedBy")]
        public Guid? CompletedBy { get; set; }

        [JsonProperty("completedAt")]
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
