using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace all_tech_webapp_service.Models.Todo.Group
{
    public class TodoGroupRequest
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class TodoGroupCreateRequest : TodoGroupRequest
    {
    }

    public class TodoGroupUpdateRequest : TodoGroupRequest
    {
        [Required]
        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}
