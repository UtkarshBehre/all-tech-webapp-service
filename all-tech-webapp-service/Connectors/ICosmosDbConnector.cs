using all_tech_webapp_service.Models.Todo;
using Microsoft.Azure.Cosmos;
using System.Linq.Expressions;
using System.Net;

namespace all_tech_webapp_service.Connectors
{
    /// <summary>
    /// CosmosDb Connector Interface
    /// </summary>
    public interface ICosmosDbConnector
    {
        /// <summary>
        /// Creates new Item in CosmosDb
        /// </summary>
        /// <typeparam name="T">Item Object Type</typeparam>
        /// <param name="item">Item</param>
        /// <param name="recordType"></param>
        /// <returns>T</returns>
        /// <exception cref="Exception"></exception>
        Task<T> CreateItemAsync<T>(T item, RecordType recordType);

        /// <summary>
        /// Reads an Item from CosmosDb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="partitionKey"></param>
        /// <param name="recordType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<T> ReadItemAsync<T>(string id, string partitionKey, RecordType recordType);

        /// <summary>
        /// Get all items from CosmosDb for a given predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recordType"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<T>> ReadItemsAsync<T>(RecordType recordType, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Updates an Item in CosmosDb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="partitionKey"></param>
        /// <param name="recordType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<T> UpdateItemAsync<T>(T item, string partitionKey, RecordType recordType, string etag);

        /// <summary>
        /// Deletes an Item with given id in CosmosDb
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="partitionKey"></param>
        /// <param name="recordType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<ItemResponse<T>> DeleteItemAsync<T>(string id, string partitionKey, RecordType recordType);
    }
}