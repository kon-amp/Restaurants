using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Restaurants.API.Filters;
using Restaurants.API.Middlewares;

namespace Restaurants.API.Extensions;
public static class DependencyInjection {
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services) {
        services
            .AddControllers(options => {
                options.Filters.Add<ValidationFilter>();
            })
            .ConfigureApiBehaviorOptions(options => {
                // Disable automatic [ApiController] model validation (DataAnnotations)
                // so you can handle validation manually or via FluentValidation.
                options.SuppressModelStateInvalidFilter = true;
            });
       

        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPISample", Version = "v1" });
        });

        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<RequestTimeLoggingMiddleware>();

        return services;
    }
}
