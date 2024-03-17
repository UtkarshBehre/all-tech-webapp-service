using all_tech_webapp_service.Models.Todo.Basic;
using all_tech_webapp_service.Models.Todo;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.User
{
    public class UserRecord : BasicRecord
    {
        [JsonProperty("googleId")]
        public string GoogleId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        public UserRecord(): base()
        {
            RecordType = RecordType.User;
        }
    }
}
