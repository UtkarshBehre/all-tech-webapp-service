using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace all_tech_webapp_service.Models.ToDoItem
{
    public class ToDoItemRecord
    {
        [JsonProperty("title")]
        public required string Title { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        public ToDoItemRecord()
        {
            Id = Guid.NewGuid();
            IsComplete = false;
            IsDeleted = false;
        }
    }
}
