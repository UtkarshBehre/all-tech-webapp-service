using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.Basic
{
    public class BasicRecord : EtagProperty
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("recordType")]
        public RecordType RecordType { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        public BasicRecord()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
        }

        public BasicRecord(Guid id)
        {
            Id = id;
            IsDeleted = false;
        }
    }
}
