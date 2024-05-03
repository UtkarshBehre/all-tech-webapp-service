using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Models.Todo;
using all_tech_webapp_service.Models.Todo.UserTodo;
using all_tech_webapp_service.Properties;
using all_tech_webapp_service.Providers.Cache;
using System.Linq.Expressions;

namespace all_tech_webapp_service.Repositories.Todo.UserTodo
{
    public class UserTodoRepository : IUserTodoRepository
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;
        private readonly ICacheProvider _cacheProvider;

        public UserTodoRepository(ICosmosDbConnector cosmosDbConnector, ICacheProvider cacheProvider)
        {
            _cosmosDbConnector = cosmosDbConnector ?? throw new ArgumentNullException(nameof(cosmosDbConnector));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        public async Task<UserTodoRecord> CreateUserTodo(UserTodoRecord userTodo)
        {
            await _cacheProvider.Delete(CacheKeysConstants.KEY_ALL_USER_TODOS);

            return await _cosmosDbConnector.CreateItemAsync(userTodo, userTodo.RecordType);
        }

        public async Task<List<UserTodoRecord>> GetAllUserTodos()
        {
            var key = CacheKeysConstants.KEY_ALL_USER_TODOS;
            var allUserTodos = await _cacheProvider.Get<List<UserTodoRecord>>(key);

            if (allUserTodos != null && allUserTodos.Count != 0)
            {
                return allUserTodos;
            }

            Expression<Func<UserTodoRecord, bool>> predicate = x
                => x.RecordType == RecordType.UserTodo &&
                   !x.IsDeleted;
            
            allUserTodos = await _cosmosDbConnector.ReadItemsAsync<UserTodoRecord>(RecordType.UserTodo, predicate);
            await _cacheProvider.Set(key, allUserTodos);
            return allUserTodos;
        }

        public async Task<UserTodoRecord> GetUserTodo(Guid id)
        {
            var key = string.Format(CacheKeysConstants.KEY_USER_TODO_ID, id);
            var userTodoRecord = await _cacheProvider.Get<UserTodoRecord>(key);

            if (userTodoRecord != null)
            {
                return userTodoRecord;
            }

            userTodoRecord = await _cosmosDbConnector.ReadItemAsync<UserTodoRecord>(id.ToString(), id.ToString(), RecordType.UserTodo);
            if (userTodoRecord == null || userTodoRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No User Todo Record Found with the given Id: {id}");
            }

            await _cacheProvider.Set(key, userTodoRecord);
            return userTodoRecord;
        }
        public async Task<UserTodoRecord> UpdateUserTodo(UserTodoRecord userTodo)
        {
            var existingUserTodo = await GetUserTodo(userTodo.Id);
            if (existingUserTodo == null || existingUserTodo.IsDeleted)
            {
                throw new FileNotFoundException($"No User Todo Record Found with the given Id: {userTodo.Id}");
            }
            var updatedUserTodo = await _cosmosDbConnector.UpdateItemAsync(userTodo, userTodo.Id.ToString(), userTodo.RecordType, userTodo.Etag);
            await _cacheProvider.Delete(string.Format(CacheKeysConstants.KEY_USER_TODO_ID, userTodo.Id));
            await _cacheProvider.Delete(CacheKeysConstants.KEY_ALL_USER_TODOS);
            return updatedUserTodo;
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
