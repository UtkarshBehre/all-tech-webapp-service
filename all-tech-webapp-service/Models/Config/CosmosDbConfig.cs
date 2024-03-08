using all_tech_webapp_service.Properties;
using System.Collections.Specialized;

namespace all_tech_webapp_service.Models.Config
{
    public class CosmosDbConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitionKey { get; set; }

        public CosmosDbConfig(ConfigurationManager configuration)
        {
            ConnectionString = configuration.GetValue<string>(Constants.COSMOSDB_CONNECTION_STRING) ?? string.Empty;
            DatabaseName = configuration.GetValue<string>(Constants.COSMOSDB_DATABASE_NAME) ?? string.Empty;
            ContainerName = configuration.GetValue<string>(Constants.COSMOSDB_CONTAINER_NAME) ?? string.Empty;
            PartitionKey = configuration.GetValue<string>(Constants.COSMOSDB_PARTITION_KEY) ?? string.Empty;
        }
    }
}
