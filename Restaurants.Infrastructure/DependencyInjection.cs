using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Application.Abstractions.User;
using Restaurants.Infrastructure.Identity;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure; 
public static class DependencyInjection {

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment) {

        services.AddRestaurantsDbContext(configuration)
                .AddIdentityLayer()
                .AddRepositories()
                .AddSeedData();
        
        return services;
    }

    public static IServiceCollection AddIdentityLayer(this IServiceCollection services) {
        services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IApplicationUser, ApplicationUser>();
        services.AddScoped<IApplicationUserStore, ApplicationUserStore>();

        return services;
    }

    public static IServiceCollection AddRestaurantsDbContext(this IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("RestaurantsDb");
        services.AddDbContext<RestaurantsDbContext>(options => 
            options.UseSqlServer(connectionString)
                   .EnableSensitiveDataLogging()
        );

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();

        return services;
    }

    public static IServiceCollection AddSeedData(this IServiceCollection services) {
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        return services;
    }
}
