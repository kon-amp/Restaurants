using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;
using System.Reflection;

namespace Restaurants.Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services) {
        var applicationAssembly = Assembly.GetExecutingAssembly();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));

        services.AddAutoMapper(cfg => { }, applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
    
