using all_tech_webapp_service.Models.Todo.Basic;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.User
{
    public class UserResponse : BasicResponse
    {
        [JsonProperty("googleId")]
        public string GoogleId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
