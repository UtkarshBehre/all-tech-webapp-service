namespace all_tech_webapp_service.Properties
{
    public class Constants
    {
        public const string COSMOSDB_CONNECTION_STRING = "CosmosDbConfig:ConnectionString";
        public const string COSMOSDB_DATABASE_NAME = "CosmosDbConfig:DatabaseName";
        public const string COSMOSDB_TODO_CONTAINER_NAME = "CosmosDbConfig:Todo:ContainerName";
        public const string COSMOSDB_TODO_PARTITION_KEY = "CosmosDbConfig:Todo:PartitionKey";
        public const string COSMOSDB_USERS_CONTAINER_NAME = "CosmosDbConfig:Users:ContainerName";
        public const string COSMOSDB_USERS_PARTITION_KEY = "CosmosDbConfig:Users:PartitionKey";
        public const string COSMOSDB_USERSTODO_CONTAINER_NAME = "CosmosDbConfig:UsersTodo:ContainerName";
        public const string COSMOSDB_USERSTODO_PARTITION_KEY = "CosmosDbConfig:UsersTodo:PartitionKey";

        public const string ISSUER = "Token:Issuer";
        public const string AUDIENCE = "Token:Audience";
        public const string APPLICATIONINSIGHTS_CONNECTION_STRING = "APPLICATIONINSIGHTS_CONNECTION_STRING";
    }
}
