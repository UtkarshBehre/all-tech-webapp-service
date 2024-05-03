using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Models.Todo;
using all_tech_webapp_service.Models.Todo.Group;
using all_tech_webapp_service.Properties;
using all_tech_webapp_service.Providers.Cache;
using System.Linq.Expressions;

namespace all_tech_webapp_service.Repositories.Todo.TodoGroupRepository
{
    public class TodoGroupRepository : ITodoGroupRepository
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;
        private readonly ICacheProvider _cacheProvider;

        public TodoGroupRepository(ICosmosDbConnector cosmosDbConnector, ICacheProvider cacheProvider)
        {
            _cosmosDbConnector = cosmosDbConnector ?? throw new ArgumentNullException(nameof(cosmosDbConnector));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        public async Task<TodoGroupRecord> CreateTodoGroup(TodoGroupRecord todoGroupRecord)
        {
            return await _cosmosDbConnector.CreateItemAsync(todoGroupRecord, todoGroupRecord.RecordType);
        }

        public async Task<TodoGroupRecord> GetTodoGroup(Guid id)
        {
            var key = string.Format(CacheKeysConstants.KEY_TODO_GROUP_ID, id);
            var todoGroupRecord = await _cacheProvider.Get<TodoGroupRecord>(key);

            if (todoGroupRecord != null)
            {
                return todoGroupRecord;
            }

            todoGroupRecord = await _cosmosDbConnector.ReadItemAsync<TodoGroupRecord>(id.ToString(), id.ToString(), RecordType.TodoGroup);
            if (todoGroupRecord == null || todoGroupRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No Todo Group Record Found with the given Id: {id}");
            }

            await _cacheProvider.Set(key, todoGroupRecord);

            return todoGroupRecord;
        }

        public async Task<List<TodoGroupRecord>> GetTodoGroups(List<Guid> ids)
        {
            Expression<Func<TodoGroupRecord, bool>> predicate = x
                => x.RecordType == RecordType.TodoGroup &&
                   ids.Contains(x.Id) &&
                   !x.IsDeleted;

            return await _cosmosDbConnector.ReadItemsAsync(RecordType.TodoGroup, predicate);
        }

        public async Task<TodoGroupRecord> UpdateTodoGroup(TodoGroupRecord todoGroupRecord)
        {
            var existingTodoGroup = await GetTodoGroup(todoGroupRecord.Id);
            if (existingTodoGroup == null || existingTodoGroup.IsDeleted)
            {
                throw new FileNotFoundException($"No Todo Group Record Found with the given Id: {todoGroupRecord.Id}");
            }
            var updatedTodoGroupRecord = await _cosmosDbConnector.UpdateItemAsync(todoGroupRecord, todoGroupRecord.Id.ToString(), todoGroupRecord.RecordType, todoGroupRecord.Etag);
            await _cacheProvider.Delete(string.Format(CacheKeysConstants.KEY_TODO_GROUP_ID, todoGroupRecord.Id));
            return updatedTodoGroupRecord;
        }

        public async Task<bool> DeleteTodoGroup(Guid id)
        {
            var todoGroupRecord = await GetTodoGroup(id);
            if (todoGroupRecord == null || todoGroupRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No Todo Group Record Found with the given Id: {id}");
            }
            todoGroupRecord.IsDeleted = true;
            await UpdateTodoGroup(todoGroupRecord);
            return true;
        }
    }
}
