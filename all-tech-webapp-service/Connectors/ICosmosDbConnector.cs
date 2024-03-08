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
        /// Creates new Item in CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T">Item Object Type</typeparam>
        /// <param name="item">Item</param>
        /// <returns>T</returns>
        /// <exception cref="Exception"></exception>
        Task<T> CreateItemAsync<T>(T item);

        /// <summary>
        /// Reads an Item from CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<T> ReadItemAsync<T>(string id);

        /// <summary>
        /// Reads an Item from CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<T> ReadItemAsync<T>(string id, string containerName, string partitionKey);

        /// <summary>
        /// Get all items from CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> ReadItemsAsync<T>();

        /// <summary>
        /// Get all items from CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        Task<List<T>> ReadItemsAsync<T>(string containerName, string partitionKey);

        /// <summary>
        /// Updates an Item in CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<T> UpdateItemAsync<T>(T item, string partitionKey);

        /// <summary>
        /// Updates an Item in CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<T> UpdateItemAsync<T>(T item, string containerName, string partitionKey);

        /// <summary>
        /// Deletes an Item with given id in CosmosDb in the DEFAULT container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<ItemResponse<T>> DeleteItemAsync<T>(string id);

        /// <summary>
        /// Deletes an Item with given id in CosmosDb in the SPECIFIED container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="containerName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<ItemResponse<T>> DeleteItemAsync<T>(string id, string containerName, string partitionKey);
    }
}