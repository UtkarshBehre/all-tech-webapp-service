using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Models.Todo;
using all_tech_webapp_service.Models.Todo.UserTodo;
using System.Linq.Expressions;

namespace all_tech_webapp_service.Repositories.Todo.UserTodo
{
    public class UserTodoRepository : IUserTodoRepository
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;

        public UserTodoRepository(ICosmosDbConnector cosmosDbConnector)
        {
            _cosmosDbConnector = cosmosDbConnector ?? throw new ArgumentNullException(nameof(cosmosDbConnector));
        }

        public async Task<UserTodoRecord> CreateUserTodo(UserTodoRecord userTodo)
        {
            return await _cosmosDbConnector.CreateItemAsync(userTodo, userTodo.RecordType);
        }

        public async Task<List<UserTodoRecord>> GetAllUserTodos()
        {
            Expression<Func<UserTodoRecord, bool>> predicate = x
                => x.RecordType == RecordType.UserTodo &&
                   !x.IsDeleted;
            ;
            var allUserTodos = await _cosmosDbConnector.ReadItemsAsync<UserTodoRecord>(RecordType.UserTodo, predicate);
            return allUserTodos;
        }

        public async Task<UserTodoRecord> GetUserTodo(Guid id)
        {
            var userTodoRecord = await _cosmosDbConnector.ReadItemAsync<UserTodoRecord>(id.ToString(), id.ToString(), RecordType.UserTodo);
            if (userTodoRecord == null || userTodoRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User Todo Record Found with the given Id: {id}");
            }
            return userTodoRecord;
        }
        public async Task<UserTodoRecord> UpdateUserTodo(UserTodoRecord userTodo)
        {
            var existingUserTodo = await GetUserTodo(userTodo.Id);
            if (existingUserTodo == null || existingUserTodo.IsDeleted)
            {
                throw new FileNotFoundException($"No User Todo Record Found with the given Id: {userTodo.Id}");
            }
            return await _cosmosDbConnector.UpdateItemAsync(userTodo, userTodo.Id.ToString(), userTodo.RecordType, userTodo.Etag);
        }

        public async Task<bool> DeleteUserTodo(Guid id)
        {
            var userTodoRecord = await GetUserTodo(id);
            if (userTodoRecord == null || userTodoRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User Todo Record Found with the given Id: {id}");
            }
            userTodoRecord.IsDeleted = true;
            await UpdateUserTodo(userTodoRecord);
            return true;
        }
    }
}
