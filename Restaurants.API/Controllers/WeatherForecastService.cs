using Microsoft.AspNetCore.Mvc;

namespace Restaurants.API.Controllers {
    public class WeatherForecastService : IWeatherForecastService {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public IEnumerable<WeatherForecast> Get(int maxResults, int minTempRange, int maxTempRange) {
            return [.. Enumerable.Range(1, maxResults).Select(index => new WeatherForecast {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(minTempRange, maxTempRange),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })];
        }
    }
}
