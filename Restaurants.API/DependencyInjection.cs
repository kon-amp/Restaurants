namespace Restaurants.API;
public static class DependencyInjection {
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration) {
        services.AddControllers();

        return services;
    }
}
