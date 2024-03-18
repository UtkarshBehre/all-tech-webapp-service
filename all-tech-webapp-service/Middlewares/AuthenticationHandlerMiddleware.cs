
using Microsoft.ApplicationInsights;

namespace all_tech_webapp_service.Middlewares
{
    public class AuthenticationHandlerMiddleware
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly RequestDelegate _next;

        public AuthenticationHandlerMiddleware(RequestDelegate next, TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"nameof(AuthenticationHandlerMiddleware) invoked");
            await _next.Invoke(context);
        }
    }
}
