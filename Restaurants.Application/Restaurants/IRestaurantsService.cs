using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants;

[Obsolete("This interface is kept only for historical reference. " +
          "The project now uses CQRS with MediatR. Avoid using this interface in new code.")]
public interface IRestaurantsService {
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants();

    Task<RestaurantDto?> GetRestaurantById(int id);

    Task<int> Create(CreateRestaurantDto createRestaurantDto);
}