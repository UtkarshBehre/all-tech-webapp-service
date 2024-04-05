using all_tech_webapp_service.Models.Chat;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace all_tech_webapp_service.Providers
{
    public class ChatHub : Hub
    {
        private readonly TelemetryClient _telemetryClient;
        public ConcurrentDictionary<string, ChatDetails> connectionIdTochatDetails;

        public ChatHub(TelemetryClient telemetryClient)
        {
            connectionIdTochatDetails = new ConcurrentDictionary<string, ChatDetails>();
            _telemetryClient = telemetryClient;
        }

        public async Task JoinChat(ChatDetails chatDetails)
        {
            await Clients.All.SendAsync("ReceiveMessage", "admin", $"{chatDetails.UserName} has joined." );
        }

        public async Task JoinSpecificChatRoom(ChatDetails chatDetails)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatDetails.ChatRoom);

            connectionIdTochatDetails[Context.ConnectionId] = chatDetails;
            _telemetryClient.TrackTrace($"ConnectionId: {Context.ConnectionId} User {chatDetails.UserName} has joined {chatDetails.ChatRoom}");
            await Clients.Group(chatDetails.ChatRoom)
                .SendAsync("ReceiveSpecificMessage", "admin", $"{chatDetails.UserName} has joined {chatDetails.ChatRoom}.");
        }

        public async Task SendMessage(string message)
        {
            if (connectionIdTochatDetails.TryGetValue(Context.ConnectionId, out ChatDetails chatDetails))
            {
                _telemetryClient.TrackTrace($"ConnectionId: {Context.ConnectionId} User {chatDetails.UserName} ChatRoom: {chatDetails.ChatRoom}");
                await Clients.Group(chatDetails.ChatRoom)
                    .SendAsync("ReceiveSpecificMessage", chatDetails.UserName, message);
            }
            else
            {
                _telemetryClient.TrackTrace($"ConnectionId not found in map");
                await Clients.Group("1")
                    .SendAsync("ReceiveSpecificMessage", "someone", message);
            }
        }
    }
}
