using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants;
internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper) : IRestaurantsService {
    public async Task<int> Create(CreateRestaurantDto createRestaurantDto) {
        logger.LogInformation("Creating a new restaurant");

        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);
        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants() {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();

        #region Manual mapping example (if not using AutoMapper)
        // var restaurantsDto = restaurants.Select(RestaurantDto.FromEntity);
        #endregion
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return restaurantsDto!;
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id) {
        logger.LogInformation("Getting restaurant if exists");
        var restaurant = await restaurantsRepository.GetByIdAsync(id);

        #region Manual mapping example (if not using AutoMapper)
        // var restaurantDto = RestaurantDto.FromEntity(restaurant);
        #endregion
        var restaurantDto = mapper.Map<RestaurantDto?>(restaurant);

        return restaurantDto;
    }
}
