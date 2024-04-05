using all_tech_webapp_service.Models.Chat;
using Microsoft.AspNetCore.SignalR;

namespace all_tech_webapp_service.Providers
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(ChatDetails chatDetails)
        {
            await Clients.All.SendAsync("ReceiveMessage", "admin", $"{chatDetails.UserName} has joined." );
        }

        public async Task JoinSpecificChatRoom(ChatDetails chatDetails)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatDetails.ChatRoom);
            await Clients.Group(chatDetails.ChatRoom)
                .SendAsync("ReceiveMessage", "admin", $"{chatDetails.UserName} has joined {chatDetails.ChatRoom}.");
        }
    }
}
