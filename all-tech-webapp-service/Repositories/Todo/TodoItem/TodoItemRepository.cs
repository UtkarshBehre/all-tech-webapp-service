using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Models.Todo;
using all_tech_webapp_service.Models.Todo.Item;
using all_tech_webapp_service.Properties;
using all_tech_webapp_service.Providers.Cache;
using AutoMapper;
using System.Linq.Expressions;

namespace all_tech_webapp_service.Repositories.Todo.TodoItem
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;
        private readonly ICacheProvider _cacheProvider;

        public TodoItemRepository(ICosmosDbConnector cosmosDbConnector, ICacheProvider cacheProvider)
        {
            _cosmosDbConnector = cosmosDbConnector ?? throw new ArgumentNullException(nameof(cosmosDbConnector));
            _cacheProvider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        /// <summary>
        /// Creates new To Do Item
        /// </summary>
        /// <param name="todoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        public async Task<TodoItemRecord> CreateTodoItem(TodoItemRecord todoItemRecord)
        {
            var createdTodoItemRecord = await _cosmosDbConnector.CreateItemAsync(todoItemRecord, todoItemRecord.RecordType);

            await _cacheProvider.Delete(CacheKeysConstants.KEY_TODO_ITEM_ALL);

            return createdTodoItemRecord;
        }

        public async Task<List<TodoItemRecord>> GetAllTodoItems()
        {
            var key = CacheKeysConstants.KEY_TODO_ITEM_ALL;
            var todoItems = await _cacheProvider.Get<List<TodoItemRecord>>(key);

            if (todoItems != null && todoItems.Count != 0)
            {
                return todoItems;
            }

            Expression<Func<TodoItemRecord, bool>> predicate = x 
                => x.RecordType == RecordType.TodoItem &&
                   !x.IsDeleted;
                                                                                            ;
            var allTodoItems = await _cosmosDbConnector.ReadItemsAsync<TodoItemRecord>(RecordType.TodoItem, predicate);
            await _cacheProvider.Set(key, allTodoItems);
            return allTodoItems;
        }

        public async Task<List<TodoItemRecord>> GetAllTodoItems(Expression<Func<TodoItemRecord, bool>> predicate)
        {
            return await _cosmosDbConnector.ReadItemsAsync<TodoItemRecord>(RecordType.TodoItem, predicate);
        }

        public async Task<List<TodoItemRecord>> GetAllTodoItemsByGroupIds(List<Guid> groupIds)
        {
            Expression<Func<TodoItemRecord, bool>> predicate = x
                => x.RecordType == RecordType.TodoItem &&
                   groupIds.Contains(x.GroupId) &&
                   !x.IsDeleted;

            var allTodoItems = await _cosmosDbConnector.ReadItemsAsync<TodoItemRecord>(RecordType.TodoItem, predicate);
            return allTodoItems;
        }

        public async Task<List<TodoItemRecord>> GetAllTodoItemByGroupId(Guid groupId)
        {
            Expression<Func<TodoItemRecord, bool>> predicate = x
                => x.RecordType == RecordType.TodoItem &&
                   x.GroupId == groupId &&
                   !x.IsDeleted;

            var allTodoItems = await _cosmosDbConnector.ReadItemsAsync<TodoItemRecord>(RecordType.TodoItem, predicate);
            return allTodoItems;
        }

        public async Task<List<TodoItemRecord>> GetAllTodoItemByGroupId(Guid groupId, bool isComplete)
        {
            Expression<Func<TodoItemRecord, bool>> predicate = x
                => x.RecordType == RecordType.TodoItem &&
                   x.GroupId == groupId &&
                   x.IsComplete == isComplete &&
                   !x.IsDeleted;

            var allTodoItems = await _cosmosDbConnector.ReadItemsAsync<TodoItemRecord>(RecordType.TodoItem, predicate);
            return allTodoItems;
        }

        /// <summary>
        /// Gets a To Do Item by Id
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        public async Task<TodoItemRecord> GetTodoItem(Guid id)
        {
            var key = string.Format(CacheKeysConstants.KEY_TODO_ITEM_ID, id);
            var todoItemRecord = await _cacheProvider.Get<TodoItemRecord>(key);
            if (todoItemRecord != null)
            {
                return todoItemRecord;
            }

            todoItemRecord = await _cosmosDbConnector.ReadItemAsync<TodoItemRecord>(id.ToString(), id.ToString(), RecordType.TodoItem);
            if (todoItemRecord == null || todoItemRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No Todo Item Record Found with the given Id: {id}");
            }

            await _cacheProvider.Set(key, todoItemRecord);

            return todoItemRecord;
        }

        /// <summary>
        /// Updates a To Do Item
        /// </summary>
        /// <param name="todoItemRecord">To Do Item Record</param>
        /// <returns></returns>
        public async Task<TodoItemRecord> UpdateTodoItem(TodoItemRecord todoItemRecord)
        {
            var existingTodoItem = await GetTodoItem(todoItemRecord.Id);
            if (existingTodoItem == null || existingTodoItem.IsDeleted)
            {
                throw new FileNotFoundException($"No Todo Item Record Found with the given Id: {todoItemRecord.Id}");
            }
            var updatedTodoItemRecord = await _cosmosDbConnector.UpdateItemAsync(todoItemRecord, todoItemRecord.Id.ToString(), todoItemRecord.RecordType, todoItemRecord.Etag);

            await _cacheProvider.Delete(string.Format(CacheKeysConstants.KEY_TODO_ITEM_ID, todoItemRecord.Id));
            await _cacheProvider.Delete(CacheKeysConstants.KEY_TODO_ITEM_ALL);
            return updatedTodoItemRecord;
        }

        /// <summary>
        /// Deletes a To Do Item
        /// </summary>
        /// <param name="id">To Do Item Id</param>
        /// <returns></returns>
        public async Task<bool> DeleteTodoItem(Guid id)
        {
            var todoItemRecord = await GetTodoItem(id);
            if (todoItemRecord == null || todoItemRecord.IsDeleted)
            {
                throw new FileNotFoundException($"No Todo Item Record Found with the given Id: {id}");
            }
            todoItemRecord.IsDeleted = true;
            await UpdateTodoItem(todoItemRecord);
            return true;
        }
    }
}
