# all-tech-webapp-service
Basic backend webapp

Add following content in appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "APPLICATIONINSIGHTS_CONNECTION_STRING": "",
  "Token": {
    "Issuer": "<< Issuer >>",
    "Audience": "<< Audience >>"
  },
  "CosmosDbConfig": {
    "ConnectionString": "<< connection string for cosmos db >>"
  },
  "RedisCache": {
    "ConnectionString": "<< connection string for redis cache >>"
  }
}