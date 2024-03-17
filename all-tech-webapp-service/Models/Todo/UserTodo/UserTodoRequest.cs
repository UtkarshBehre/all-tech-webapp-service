using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace all_tech_webapp_service.Models.Todo.UserTodo
{
    public class UserTodoRequest
    {
        [Required]
        [JsonProperty("groupIds")]
        public List<Guid> GroupIds { get; set; }
    }

    public class  UserTodoCreateRequest : UserTodoRequest
    {
        [Required]
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class UserTodoUpdateRequest : UserTodoRequest
    {
        [Required]
        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}
