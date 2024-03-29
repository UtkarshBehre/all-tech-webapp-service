﻿
using all_tech_webapp_service.Providers;
using Microsoft.ApplicationInsights;

namespace all_tech_webapp_service.Middlewares
{
    public class AuthenticationHandlerMiddleware
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly RequestDelegate _next;
        private readonly ITokenHandlerProvider _tokenHandleProvider;

        public AuthenticationHandlerMiddleware(RequestDelegate next, ITokenHandlerProvider tokenHandleProvider, TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _tokenHandleProvider = tokenHandleProvider ?? throw new ArgumentNullException(nameof(tokenHandleProvider));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(token))
            {
                _telemetryClient.TrackTrace("Token is missing");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            _telemetryClient.TrackTrace($"Token: {token}");
            _tokenHandleProvider.SetToken(token);
            _tokenHandleProvider.ValidateToken();
            await _next.Invoke(context);
        }
    }
}
