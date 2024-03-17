using all_tech_webapp_service.Models.Todo.Basic;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Todo.Group
{
    public class TodoGroupResponse : BasicResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
