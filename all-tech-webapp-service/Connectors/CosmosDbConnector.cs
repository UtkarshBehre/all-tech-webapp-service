using all_tech_webapp_service.Models.Config;
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
        private readonly string _containerName;
        private readonly string _partitionKey;

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
            _containerName = cosmosDbConfig.ContainerName;
            _partitionKey = cosmosDbConfig.PartitionKey;
        }

        private async Task<Database> GetDatabase()
        {
            return await _client.CreateDatabaseIfNotExistsAsync(_databaseName);
        }

        private async Task<Container> GetContainer(string containerName, string partitionKey)
        {
            var database = await GetDatabase();
            return await database.CreateContainerIfNotExistsAsync(containerName, partitionKey);
        }

        private async Task<Container> GetContainer()
        {
            var database = await GetDatabase();
            return await database.CreateContainerIfNotExistsAsync(_containerName, _partitionKey);
        }

        /// <summary>
        /// Creates new Item in CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T">Item Object Type</typeparam>
        /// <param name="item">Item</param>
        /// <returns>T</returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> CreateItemAsync<T>(T item)
        {
            var container = await GetContainer();
            var itemResponse = await container.CreateItemAsync(item);
            if (itemResponse.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception($"Failed to create item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        /// <summary>
        /// Reads an Item from CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> ReadItemAsync<T>(string id)
        {
            var container = await GetContainer();
            var itemResponse = await container.ReadItemAsync<T>(id, new PartitionKey(id));
            if (itemResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to process item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        /// <summary>
        /// Reads an Item from CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> ReadItemAsync<T>(string id, string containerName, string partitionKey)
        {
            var container = await GetContainer(containerName, partitionKey);
            var itemResponse = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
            if (itemResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to process item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        /// <summary>
        /// Get all items from CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> ReadItemsAsync<T>()
        {
            var container = await GetContainer();
            var iterator = container.GetItemLinqQueryable<T>().ToFeedIterator();
            var items = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                items.AddRange(response);
            }
            return items;
        }

        /// <summary>
        /// Get all items from CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        public async Task<List<T>> ReadItemsAsync<T>(string containerName, string partitionKey)
        {
            var container = await GetContainer(containerName, partitionKey);
            var iterator = container.GetItemLinqQueryable<T>().ToFeedIterator();
            var items = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                items.AddRange(response);
            }
            return items;
        }

        /// <summary>
        /// Updates an Item in CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> UpdateItemAsync<T>(T item, string partitionKey)
        {
            var container = await GetContainer();
            var itemResponse = await container.UpsertItemAsync(item, new PartitionKey(partitionKey));
            if (itemResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to update item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        /// <summary>
        /// Updates an Item in CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> UpdateItemAsync<T>(T item, string containerName, string partitionKey)
        {
            var container = await GetContainer(containerName, partitionKey);
            var itemResponse = await container.UpsertItemAsync(item, new PartitionKey(partitionKey));
            if (itemResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to update item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse.Resource;
        }

        /// <summary>
        /// Deletes an Item with given id in CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ItemResponse<T>> DeleteItemAsync<T>(string id)
        {
            var container = await GetContainer();
            var itemResponse = await container.DeleteItemAsync<T>(id, new PartitionKey(_partitionKey));
            if (itemResponse.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception($"Failed to delete item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse;
        }

        /// <summary>
        /// Deletes an Item with given id in CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ItemResponse<T>> DeleteItemAsync<T>(string id, string containerName, string partitionKey)
        {
            var container = await GetContainer(containerName, partitionKey);
            var itemResponse = await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
            if (itemResponse.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception($"Failed to delete item: {itemResponse.StatusCode} | {itemResponse.Diagnostics}");
            }
            return itemResponse;
        }
    }
}
