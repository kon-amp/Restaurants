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
            // Define a Swagger document (name + metadata)
            c.SwaggerDoc("v1", new OpenApiInfo { 
                Title = "WebAPISample", 
                Version = "v1" 
            });

            // Tell Swagger how our authentication works (Bearer token in Authorization header)
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme {
                Type = SecuritySchemeType.Http,                     // This is an HTTP auth scheme
                Scheme = "Bearer",                                  // Use "Bearer <token>"
                BearerFormat = "JWT"                                // (optional) indicates it's a JWT token
            });

            // Require this BearerAuth scheme for all endpoints unless overridden
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,    // points back to the scheme above
                            Id = "bearerAuth"                       // the name we used earlier
                        }
                    },
                    Array.Empty<string>() // no specific scopes required (for JWT)
                }
            });
        });

        // Identity Endpoints are minimalAPI so we have to make them visible
        services.AddEndpointsApiExplorer();

        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<RequestTimeLoggingMiddleware>();

        return services;
    }
}
