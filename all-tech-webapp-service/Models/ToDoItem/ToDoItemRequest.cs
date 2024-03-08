using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace all_tech_webapp_service.Models.ToDoItem
{
    public class ToDoItemRequest
    {
        [JsonProperty("taskTitle")]
        [Required]
        public required string TaskTitle { get; set; }
    }

    public class ToDoItemCreateRequest : ToDoItemRequest
    {
    }

    public class ToDoItemUpdateRequest : ToDoItemRequest
    {
        [JsonProperty("id")]
        [Required]
        public Guid Id { get; set; }

        [JsonProperty("isComplete")]
        [Required]
        public bool IsComplete { get; set; }
    }
}
