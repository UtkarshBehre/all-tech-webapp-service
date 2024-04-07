namespace all_tech_webapp_service.Providers.Todo
{
    public interface ITodoGroupHub
    {
        Task SendTodoGroupSharedMessage(string fullName, string groupName, string userIdSharedWith);
    }
}
