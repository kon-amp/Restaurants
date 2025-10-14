using Microsoft.OpenApi.Models;

namespace Restaurants.API;
public static class DependencyInjection {
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration) {
        services.AddControllers();
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPISample", Version = "v1" });
        });

        return services;
    }
}
