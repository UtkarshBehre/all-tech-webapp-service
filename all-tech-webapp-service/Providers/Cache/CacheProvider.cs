using Newtonsoft.Json;
using StackExchange.Redis;

namespace all_tech_webapp_service.Providers.Cache
{
    public class CacheProvider : ICacheProvider
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly IDatabase _cache;
        private readonly bool _localEnvironment;
        
        public CacheProvider(string connectionString, bool localEnvironment)
        {
            _connection = ConnectionMultiplexer.Connect(connectionString) ?? throw new Exception("Could not connect to cache");
            _localEnvironment = localEnvironment;
            _cache = _connection.GetDatabase() ?? throw new Exception("Could not connect to cache and get Database");
        }

        public async Task<T> Get<T>(string key)
        {
#pragma warning disable CS8603 // Possible null reference return.
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_localEnvironment)
            {
                key = $"local-{key}";
            }

            string? value = await _cache.StringGetAsync(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task Set(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var sValue = JsonConvert.SerializeObject(value);
            
            if (_localEnvironment)
            {
                key = $"local-{key}";
            }
            
            await _cache.StringSetAsync(key, sValue);
        }

        public async Task<bool> Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_localEnvironment)
            {
                key = $"local-{key}";
            }

            return await _cache.KeyDeleteAsync(key);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
