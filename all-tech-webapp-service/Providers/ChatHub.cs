using all_tech_webapp_service.Models.Chat;
using Microsoft.AspNetCore.SignalR;

namespace all_tech_webapp_service.Providers
{
    public class ChatHub : Hub
    {
        public Dictionary<string, ChatDetails> connectionIdTochatDetails;

        public ChatHub()
        {
            connectionIdTochatDetails = new Dictionary<string, ChatDetails>();
        }

        public async Task JoinChat(ChatDetails chatDetails)
        {
            await Clients.All.SendAsync("ReceiveMessage", "admin", $"{chatDetails.UserName} has joined." );
        }

        public async Task JoinSpecificChatRoom(ChatDetails chatDetails)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatDetails.ChatRoom);

            connectionIdTochatDetails.Add(Context.ConnectionId, chatDetails);

            await Clients.Group(chatDetails.ChatRoom)
                .SendAsync("ReceiveSpecificMessage", "admin", $"{chatDetails.UserName} has joined {chatDetails.ChatRoom}.");
        }

        public async Task SendMessage(string message)
        {
            if (connectionIdTochatDetails.TryGetValue(Context.ConnectionId, out ChatDetails chatDetails))
            {
                await Clients.Group(chatDetails.ChatRoom)
                    .SendAsync("ReceiveSpecificMessage", chatDetails.UserName, message);
            }
        }
    }
}
