using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.User
{
    public class UserRequest 
    {
        [Required]
        [JsonProperty("googleId")]
        public string GoogleId { get; set; }

        [MaxLength(32)]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [MaxLength(32)]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class  UserCreateRequest : UserRequest
    {

    }

    public class UserUpdateRequest : UserRequest
    {
        [Required]
        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}
