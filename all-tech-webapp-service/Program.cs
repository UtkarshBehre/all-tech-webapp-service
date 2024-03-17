using all_tech_webapp_service.Connectors;
using all_tech_webapp_service.Models.Config;
using all_tech_webapp_service.Properties;
using all_tech_webapp_service.Providers;
using all_tech_webapp_service.Repositories.Todo.TodoGroupRepository;
using all_tech_webapp_service.Repositories.Todo.TodoItem;
using all_tech_webapp_service.Repositories.Todo.UserTodo;
using all_tech_webapp_service.Repositories.User;
using all_tech_webapp_service.Services;
using all_tech_webapp_service.Services.Todo.Group;
using all_tech_webapp_service.Services.Todo.Item;
using all_tech_webapp_service.Services.Todo.UserTodo;
using all_tech_webapp_service.Services.User;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using System.Text;
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

            //var cosmosDbConfig = builder.Environment.IsDevelopment() ? new CosmosDbConfig(builder.Configuration) : new CosmosDbConfig(ConfigurationManager.AppSettings);
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
        }

        public static void SetupAuth(IServiceCollection services)
        {
            string Issuer = "infologs.in";
            string Audience = "global";
                
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("OnTokenValidated");
                        if (context.Request.Headers.ContainsKey("JWTToken"))
                        {
                            var Token = context.Request.Headers["JWTToken"].ToString();
                            if (Token.StartsWith("Bearer"))
                            {
                                var token = Token.Substring(7).ToString();
                            }
                        }

                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        Console.WriteLine("OnMessageReceived");
                        if (context.Request.Headers.ContainsKey("JWTToken"))
                        {
                            var Token = context.Request.Headers["JWTToken"].ToString();
                            if (Token.StartsWith("Bearer"))
                            {
                                context.Token = Token.Substring(7).ToString();
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("OnAuthenticationFailed");
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.ContentType = context.Request.Headers["Accept"].ToString();
                        string _Message = "Authentication token is invalid.";
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            //context.Response.Headers.Add("Token-Expired", "true");
                            //OR
                            _Message = "Token has expired.";
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {
                                StatusCode = (int)HttpStatusCode.Unauthorized,
                                Message = _Message
                            }));
                        }
                        return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            StatusCode = (int)HttpStatusCode.Unauthorized,
                            Message = _Message
                        }));
                        //return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
