﻿using Microsoft.ApplicationInsights;

namespace all_tech_webapp_service.Services
{
    public interface IWeatherForecastService
    {
        WeatherForecast[] getWeatherForecasts();
    }
}