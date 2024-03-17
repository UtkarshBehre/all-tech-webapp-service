using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.Basic
{
    public class BasicResponse : EtagProperty
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
