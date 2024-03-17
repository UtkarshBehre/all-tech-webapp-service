using all_tech_webapp_service.Properties;
using System.Collections.Specialized;

namespace all_tech_webapp_service.Models.Config
{
    public class CosmosDbConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TodoItemsContainerName { get; private set; }
        public string TodoItemsPartitionKey { get; private set; }
        public string UsersContainerName { get; private set; }
        public string UsersPartitionKey { get; private set; }
        public string UsersTodoContainerName { get; private set; }
        public string UsersTodoPartitionKey { get; private set; }

        public CosmosDbConfig(ConfigurationManager configuration)
        {
            ConnectionString = configuration.GetValue<string>(Constants.COSMOSDB_CONNECTION_STRING) ?? string.Empty;
            DatabaseName = configuration.GetValue<string>(Constants.COSMOSDB_DATABASE_NAME) ?? string.Empty;
            TodoItemsContainerName = configuration.GetValue<string>(Constants.COSMOSDB_TODO_CONTAINER_NAME) ?? string.Empty;
            TodoItemsPartitionKey = configuration.GetValue<string>(Constants.COSMOSDB_TODO_PARTITION_KEY) ?? string.Empty;
            UsersContainerName = configuration.GetValue<string>(Constants.COSMOSDB_USERS_CONTAINER_NAME) ?? string.Empty;
            UsersPartitionKey = configuration.GetValue<string>(Constants.COSMOSDB_USERS_PARTITION_KEY) ?? string.Empty;
            UsersTodoContainerName = configuration.GetValue<string>(Constants.COSMOSDB_USERSTODO_CONTAINER_NAME) ?? string.Empty;
            UsersTodoPartitionKey = configuration.GetValue<string>(Constants.COSMOSDB_USERSTODO_PARTITION_KEY) ?? string.Empty;
        }
    }
}
