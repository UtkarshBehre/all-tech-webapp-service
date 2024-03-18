using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace all_tech_webapp_service.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next, TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (BadHttpRequestException ex)
            {
                var error = $"{context.Request.Path}: {ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, SeverityLevel.Information);
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status400BadRequest;
                var problemDetails = new ProblemDetails
                {
                    Title = "Bad Request",
                    Status = response.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                await response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
            catch (Exception ex) when (ex is SecurityTokenExpiredException ||
                                       ex is SecurityTokenInvalidAudienceException ||
                                       ex is SecurityTokenInvalidIssuerException)
            {
                var error = $"{context.Request.Path}: {ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, SeverityLevel.Information);
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status401Unauthorized;
                var problemDetails = new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = response.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                await response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
            catch (FileNotFoundException ex)
            {
                var error = $"{context.Request.Path}: {ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, SeverityLevel.Information);
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status404NotFound;
                var problemDetails = new ProblemDetails
                {
                    Title = "Resource not found",
                    Status = response.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                await response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
            catch (CosmosException ex)
            {
                var error = $"{context.Request.Path}: {ex.Message}. StackTrace: {ex.StackTrace}";
                _telemetryClient.TrackTrace(error, SeverityLevel.Information);
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)ex.StatusCode;
                var problemDetails = new ProblemDetails
                {
                    Title = "An error during transaction occurred",
                    Status = response.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                await response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
            catch (InvalidOperationException ex)
            {
                _telemetryClient.TrackException(ex);
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status500InternalServerError;
                var problemDetails = new ProblemDetails
                {
                    Title = "An invalid operation occurred",
                    Status = response.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                await response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status500InternalServerError;
                var problemDetails = new ProblemDetails
                {
                    Title = "An internal error occurred",
                    Status = response.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                await response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }
        }
    }

    public class ProblemDetails
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("detail")]
        public string Detail { get; set; }
        [JsonProperty("instance")]
        public string Instance { get; set; }
    }
}
