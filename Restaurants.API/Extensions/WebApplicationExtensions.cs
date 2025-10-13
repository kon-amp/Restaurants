using Restaurants.Infrastructure.Seeders;
using Restaurants.Application;
using Restaurants.Infrastructure;

namespace Restaurants.API.Extensions;
internal static class WebApplicationExtensions {
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder) {
        var configuration = builder.Configuration;

        // Add clean architecture layers.
        builder.Services
               .AddPresentationLayer(configuration)
               .AddApplicationLayer(configuration)
               .AddInfrastructureLayer(configuration, builder.Environment);

        // Build a WebApplication instance
        return builder.Build();
    }

    internal static async Task<WebApplication> ConfigurePipeline(this WebApplication app) {
        var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

        await seeder.Seed();
        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
