using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure; 
public static class DependencyInjection {

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, ConfigurationManager configuration, IHostEnvironment hostEnvironment) {

        services.AddRestaurantsDbContext(configuration)
                .AddRepositories()
                .AddSeedData();

        return services;
    }

    public static IServiceCollection AddRestaurantsDbContext(this IServiceCollection services, ConfigurationManager configuration) {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();

        return services;
    }

    public static IServiceCollection AddSeedData(this IServiceCollection services) {
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        return services;
    }
}
