using all_tech_webapp_service.Models.Config;
using all_tech_webapp_service.Models.Todo;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using System.Net;

namespace all_tech_webapp_service.Connectors
{
    /// <summary>
    /// ComosDb Connector Interface
    /// </summary>
    public class CosmosDbConnector : ICosmosDbConnector
    {
        private readonly CosmosClient _client;
        private readonly string _databaseName;
        private readonly string TODO_ITEMS_CONTAINER_NAME;
        private readonly string TODO_ITEMS_PARTITION_KEY;
        private readonly string USERS_CONTAINER_NAME;
        private readonly string USERS_PARTITION_KEY;
        private readonly string USERS_TODO_CONTAINER_NAME;
        private readonly string USERS_TODO_PARTITION_KEY;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="databaseName">Database Name</param>
        /// <param name="containerName">Container Name</param>
        /// <param name="partitionKey">Partition Key</param>
        /// <exception cref="ArgumentException"></exception>
        public CosmosDbConnector(CosmosDbConfig cosmosDbConfig)
        {
            _client = new CosmosClient(cosmosDbConfig.ConnectionString) ?? throw new ArgumentException($"Unable to cosmos db client because following found as incorrect : {nameof(cosmosDbConfig.ConnectionString)}");
            _databaseName = cosmosDbConfig.DatabaseName;
            
            TODO_ITEMS_CONTAINER_NAME = cosmosDbConfig.TodoItemsContainerName;
            TODO_ITEMS_PARTITION_KEY = cosmosDbConfig.TodoItemsPartitionKey;
            USERS_CONTAINER_NAME = cosmosDbConfig.UsersContainerName;
            USERS_PARTITION_KEY = cosmosDbConfig.UsersPartitionKey;
            USERS_TODO_CONTAINER_NAME = cosmosDbConfig.UsersTodoContainerName;
            USERS_TODO_PARTITION_KEY = cosmosDbConfig.UsersTodoPartitionKey;
        }

        private async Task<Database> GetDatabase()
        {
            ThroughputProperties autoscaleThroughputProperties = ThroughputProperties.CreateAutoscaleThroughput(1000);
            return await _client.CreateDatabaseIfNotExistsAsync(_databaseName, throughputProperties: autoscaleThroughputProperties);
        }

        private async Task<Container> GetContainer(string containerName, string partitionKey)
        {
            var database = await GetDatabase();
            return await database.CreateContainerIfNotExistsAsync(containerName, partitionKey);
        }

        private async Task<Container> GetContainer(RecordType recordType)
        {
            return recordType switch
            {
                RecordType.TodoItem or RecordType.TodoGroup => await GetContainer(TODO_ITEMS_CONTAINER_NAME, TODO_ITEMS_PARTITION_KEY),
                RecordType.UserTodo => await GetContainer(USERS_TODO_CONTAINER_NAME, USERS_TODO_PARTITION_KEY),
                RecordType.User => await GetContainer(USERS_CONTAINER_NAME, USERS_PARTITION_KEY),
                _ => await GetContainer(TODO_ITEMS_CONTAINER_NAME, TODO_ITEMS_PARTITION_KEY),
            };
        }

        public async Task<T> CreateItemAsync<T>(T item, RecordType recordType)
        {
            var container = await GetContainer(recordType);
            var itemResponse = await container.CreateItemAsync(item);
            if (itemResponse.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Failed to create item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        public async Task<T> ReadItemAsync<T>(string id, string partitionKey, RecordType recordType)
        {
            var container = await GetContainer(recordType);
            var itemResponse = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
            if (itemResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to process item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        public async Task<List<T>> ReadItemsAsync<T>(RecordType recordType, Expression<Func<T, bool>> predicate)
        {
            var container = await GetContainer(recordType);
            var iterator = container.GetItemLinqQueryable<T>().Where(predicate) .ToFeedIterator();
            var items = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                items.AddRange(response);
            }
            return items;
        }

        public async Task<T> UpdateItemAsync<T>(T item, string partitionKey, RecordType recordType, string etag)
        {
            var container = await GetContainer(recordType);

            ItemRequestOptions itemRequestOptions = new ItemRequestOptions
            {
                IfMatchEtag = etag
            };

            var itemResponse = await container.UpsertItemAsync(item, new PartitionKey(partitionKey), requestOptions: itemRequestOptions);
            if (itemResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to update item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        public async Task<ItemResponse<T>> DeleteItemAsync<T>(string id, string partitionKey, RecordType recordType)
        {
            var container = await GetContainer(recordType);
            var itemResponse = await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
            if (itemResponse.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception($"Failed to delete item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse;
        }
    }
}
