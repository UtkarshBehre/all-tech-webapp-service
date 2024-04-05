using Newtonsoft.Json;

namespace all_tech_webapp_service.Models.Chat
{
    public class ChatDetails
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("chatRoom")]
        public string ChatRoom { get; set; }
    }
}
