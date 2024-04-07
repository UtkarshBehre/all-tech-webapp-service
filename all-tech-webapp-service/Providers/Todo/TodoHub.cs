using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.SignalR;

namespace all_tech_webapp_service.Providers.Todo
{
    public class TodoHub : Hub
    {
        private readonly TelemetryClient _telemetryClient;

        public TodoHub(TelemetryClient telemetryClient) 
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public async Task JoinTodoGroupHub(string groupId)
        {
            _telemetryClient.TrackTrace($"{Context.ConnectionId} joined todo group hub {groupId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task JoinUserHub(string userId)
        {
            _telemetryClient.TrackTrace($"{Context.ConnectionId} joined user hub {userId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        public async Task SendTodoGroupUpdatedMessage(string groupId, string groupName, string name)
        {
            string message = $"{name} updated the todo list group {groupName}";
            _telemetryClient.TrackTrace($"{groupId}: {message}");
            await Clients.GroupExcept(groupId, Context.ConnectionId)
                .SendAsync("ReceiveTodoGroupUpdates", message);
        }

        // Only for Backend
        public async Task SendTodoGroupSharedMessage(string fullName, string groupName, string userIdSharedWith)
        {
            string message = $"{fullName} shared a todo list group {groupName} with you.";
            _telemetryClient.TrackTrace($"{userIdSharedWith}: {message}");
            await Clients.Group(userIdSharedWith)
                .SendAsync("ReceiveTodoGroupShares", message);
        }
    }
}
