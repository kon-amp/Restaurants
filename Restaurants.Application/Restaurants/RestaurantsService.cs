using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants;
internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger) : IRestaurantsService {
    public async Task<IEnumerable<Restaurant>> GetAllRestaurants() {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetRestaurantById(int id) {
        logger.LogInformation("Getting restaurant if exists");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);
        return restaurant;
    }
}
