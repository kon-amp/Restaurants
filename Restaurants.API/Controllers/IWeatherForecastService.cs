
namespace Restaurants.API.Controllers {
    public interface IWeatherForecastService {
        IEnumerable<WeatherForecast> Get(int maxResults, int minTempRange, int maxTempRange);
    }

    
}