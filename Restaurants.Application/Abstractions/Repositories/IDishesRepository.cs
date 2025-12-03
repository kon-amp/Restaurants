using Restaurants.Domain.Entities;

namespace Restaurants.Application.Abstractions.Repositories; 
public interface IDishesRepository {
    Task<int> Create(Dish entity);

    Task Delete(IEnumerable<Dish> entities);
}
