namespace all_tech_webapp_service.Providers.Todo
{
    public interface ITodoHub
    {
        Task SendTodoGroupSharedMessage(string fullName, string groupName, string userIdSharedWith);
    }
}
