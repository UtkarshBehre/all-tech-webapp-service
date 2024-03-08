using all_tech_webapp_service.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace all_tech_webapp_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly TelemetryClient _telemetryClient;

        public WeatherForecastController(IWeatherForecastService weatherForecastService, TelemetryClient telemetryClient)
        {
            _weatherForecastService = weatherForecastService ?? throw new ArgumentNullException(nameof(weatherForecastService));
            _telemetryClient = telemetryClient;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _telemetryClient.TrackTrace($"{nameof(WeatherForecastController)}.{nameof(Get)}");
            var weatherForecasts = Array.Empty<WeatherForecast>();
            
            try
            {
                weatherForecasts = _weatherForecastService.GetWeatherForecasts();
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
            }

            return weatherForecasts;
        }
    }
}
