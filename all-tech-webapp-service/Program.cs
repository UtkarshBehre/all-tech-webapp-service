using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Models;
using all_tech_webapp_service.Properties;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.ToDoList;
using all_tech_webapp_service.Services;
using all_tech_webapp_service.Services.ToDoList;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace all_tech_webapp_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // SETUP APP INSIGHTS
            var asConnectionString = ConfigurationManager.AppSettings[Constants.APPLICATIONINSIGHTS_CONNECTION_STRING] ?? string.Empty;
            builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions { ConnectionString = asConnectionString });

            // SETUP COSMOS DB
            var cosmosDbConfig = new CosmosDbConfig(builder.Configuration);
            var cosmosDbConnector = new CosmosDbConnector(cosmosDbConfig);

            builder.Services.AddSingleton<ICosmosDbConnector>(x => cosmosDbConnector);
            
            AddServices(builder);
            var app = builder.Build();


            // SETUP SWAGGER FOR ALL ENVIRONMENTS
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static string? GetAppSettingValue(WebApplicationBuilder builder, string key)
        {
            return builder.Configuration.GetValue<string>(key);
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IAutoMapperProvider, AutoMapperProvider>();

            builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
            builder.Services.AddScoped<IToDoListService, ToDoListService>();
        }
    }
}
