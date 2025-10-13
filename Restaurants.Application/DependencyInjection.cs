using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration) {
        services.AddScoped<IRestaurantsService, RestaurantsService>();

        return services;
    }
}
    
