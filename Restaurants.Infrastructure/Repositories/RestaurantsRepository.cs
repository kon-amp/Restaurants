using Microsoft.EntityFrameworkCore;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;
internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository {
    public async Task<IEnumerable<Restaurant>> GetAllAsync() {
        var restaurants = await dbContext.Restaurants.ToListAsync();
        return restaurants;
    }
}
