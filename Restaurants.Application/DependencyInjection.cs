using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Abstractions.User;
using Restaurants.Application.User;
using System.Reflection;

namespace Restaurants.Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services) {
        var applicationAssembly = Assembly.GetExecutingAssembly();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));

        services.AddAutoMapper(cfg => { }, applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

        return services;
    }
}
    
