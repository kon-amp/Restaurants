using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants;

/// <summary>
/// ⚠️ Obsolete: This service is kept only for historical reference.
/// </summary>
/// <remarks>
/// This class represents the traditional "Service Layer" approach that directly handles
/// business logic and data access through repositories.
/// 
/// It has been replaced in the current architecture by the <b>CQRS pattern</b>
/// using the <b>MediatR</b> library for improved separation of concerns.
/// 
/// You may refer to this class only if you intentionally wish to avoid CQRS
/// and work with a simplified service-based structure.
/// </remarks>
[Obsolete("This service is kept only for historical reference. " +
          "The project now uses CQRS with MediatR. Avoid using this service in new code.")]
internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper) : IRestaurantsService {

    /// <summary>
    /// Creates a new restaurant record.
    /// </summary>
    /// <param name="createRestaurantDto">The DTO containing restaurant creation data.</param>
    /// <returns>The ID of the newly created restaurant.</returns>
    public async Task<int> Create(CreateRestaurantDto createRestaurantDto) {
        logger.LogInformation("Creating a new restaurant");

        var restaurant = mapper.Map<Restaurant>(createRestaurantDto);
        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }

    /// <summary>
    /// Retrieves all restaurants.
    /// </summary>
    /// <remarks>
    /// This method demonstrates both AutoMapper usage and an example of manual mapping.
    /// </remarks>
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants() {
        logger.LogInformation("Getting all restaurants");
        var restaurants = await restaurantsRepository.GetAllAsync();

        #region Manual mapping example (if not using AutoMapper)
        // var restaurantsDto = restaurants.Select(RestaurantDto.FromEntity);
        #endregion
        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return restaurantsDto!;
    }

    /// <summary>
    /// Retrieves a restaurant by its unique identifier.
    /// </summary>
    /// <param name="id">The restaurant's ID.</param>
    /// <returns>A <see cref="RestaurantDto"/> if found, otherwise <c>null</c>.</returns>
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
