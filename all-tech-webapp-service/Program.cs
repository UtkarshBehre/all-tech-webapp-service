using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Middlewares;
using all_tech_webapp_service.Models.Config;
using all_tech_webapp_service.Properties;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Providers.Todo;
using all_tech_webapp_service.Repositories.Todo.TodoGroupRepository;
using all_tech_webapp_service.Repositories.Todo.TodoItem;
using all_tech_webapp_service.Repositories.Todo.UserTodo;
using all_tech_webapp_service.Repositories.User;
using all_tech_webapp_service.Services;
using all_tech_webapp_service.Services.Todo.DashBoard;
using all_tech_webapp_service.Services.Todo.Group;
using all_tech_webapp_service.Services.Todo.Item;
using all_tech_webapp_service.Services.Todo.UserTodo;
using all_tech_webapp_service.Services.User;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.SignalR;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace all_tech_webapp_service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSignalR();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllTechApp", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            // SETUP APP INSIGHTS
            var asConnectionString = ConfigurationManager.AppSettings[Constants.APPLICATIONINSIGHTS_CONNECTION_STRING] ?? string.Empty;
            builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions { ConnectionString = asConnectionString });

            // SETUP COSMOS DB

            //var cosmosDbConfig = builder.Environment.IsDevelopment() ? new CosmosDbConfig(builder.Configuration) : new CosmosDbConfig(ConfigurationManager.AppSettings);
            var cosmosDbConfig = new CosmosDbConfig(builder.Configuration);
            var cosmosDbConnector = new CosmosDbConnector(cosmosDbConfig);

            builder.Services.AddSingleton<ICosmosDbConnector>(x => cosmosDbConnector);

            var tokenHandlerProvider = new TokenHandlerProvider(builder.Environment, builder.Configuration);
            builder.Services.AddSingleton<ITokenHandlerProvider>(x => tokenHandlerProvider);

            AddServices(builder);

            var app = builder.Build();

            // SETUP SWAGGER FOR ALL ENVIRONMENTS
            app.UseCors("AllTechApp");

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseMiddleware<AuthenticationHandlerMiddleware>();
            
            app.UseHttpsRedirection();
            
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<TodoHub>(Constants.TodoHubPath);
            app.MapHub<ChatHub>(Constants.ChatHubPath);


            app.Run();
        }

        public static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IAutoMapperProvider, AutoMapperProvider>();

            // repos
            builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
            builder.Services.AddScoped<ITodoGroupRepository, TodoGroupRepository>();
            builder.Services.AddScoped<IUserTodoRepository, UserTodoRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // services
            builder.Services.AddScoped<ITodoItemService, TodoItemService>();
            builder.Services.AddScoped<ITodoGroupService, TodoGroupService>();
            builder.Services.AddScoped<IUserTodoService, UserTodoService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
