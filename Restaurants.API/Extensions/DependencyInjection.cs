using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Restaurants.API.Filters;

namespace Restaurants.API.Extensions;
public static class DependencyInjection {
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration) {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });

        // Disable automatic [ApiController] model validation (DataAnnotations)
        // so you can handle validation manually or via FluentValidation.
        services.Configure<ApiBehaviorOptions>(options => {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPISample", Version = "v1" });
        });

        return services;
    }
}
