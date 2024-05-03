namespace all_tech_webapp_service.Providers.Cache
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Get value from cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> Get<T>(string key);

        /// <summary>
        /// Set Key Value in cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task Set(string key, object value);

        /// <summary>
        /// Delete a key value from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> Delete(string key);

        /// <summary>
        /// Dispose the cache connection
        /// </summary>
        void Dispose();
    }
}
