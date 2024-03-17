using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace all_tech_webapp_service.Models.Todo.Item
{
    public class TodoItemRequest
    {
        [Required]
        [JsonProperty("groupId")]
        public Guid GroupId { get; set; }

        [Required]
        [JsonProperty("title")]
        public required string Title { get; set; }
    }

    public class TodoItemCreateRequest : TodoItemRequest
    {
    }

    public class TodoItemUpdateRequest : TodoItemRequest
    {
        [Required]
        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        [Required]
        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}
