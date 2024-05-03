using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.Basic
{
    public class BasicResponse : EtagProperty
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonProperty("lastModifiedAt")]
        public DateTimeOffset? LastModifiedAt { get; set; }

        [JsonProperty("lastModifiedBy")]
        public Guid LastModifiedBy { get; set; }
    }
}
