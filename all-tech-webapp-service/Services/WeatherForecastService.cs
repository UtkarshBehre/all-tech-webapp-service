using Microsoft.ApplicationInsights;

namespace all_tech_webapp_service.Services
{
    public class WeatherForecastService: IWeatherForecastService
    {
        private readonly TelemetryClient _telemetryClient;

        public WeatherForecastService(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public WeatherForecast[] getWeatherForecasts()
        {
            _telemetryClient.TrackTrace($"{nameof(WeatherForecastService)}.{nameof(getWeatherForecasts)}");

            var Summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
