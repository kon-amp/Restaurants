using Restaurants.Domain.Entities;

namespace Restaurants.Application.Abstractions.Repositories;
public interface IRestaurantsRepository {
    Task<IEnumerable<Restaurant>> GetAllAsync();

    Task<Restaurant?> GetByIdAsync(int id);
}
