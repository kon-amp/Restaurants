using Restaurants.Infrastructure.Seeders;
using Restaurants.Application;
using Restaurants.Infrastructure;
using Serilog;
using Serilog.Events;

namespace Restaurants.API.Extensions;
internal static class WebApplicationExtensions {
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder) {
        var configuration = builder.Configuration;

        // Add clean architecture layers.
        builder.Services
               .AddPresentationLayer()
               .AddApplicationLayer()
               .AddInfrastructureLayer(configuration, builder.Environment);

        // Add Logging Mechanism
        builder.Host.UseSerilog((context, configuration) => 
            configuration.ReadFrom.Configuration(context.Configuration)
        );

        // Build a WebApplication instance
        return builder.Build();
    }

    internal static async Task<WebApplication> ConfigurePipeline(this WebApplication app) {
        // Create a temporary DI scope.
        // This allows us to safely resolve and use scoped services (like DbContext)
        // outside of a normal HTTP request.
        using (var scope = app.Services.CreateScope()) {
            var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

            // Run database seeding (e.g., initial data creation).
            await seeder.Seed();
        }
        
        // --- Configure the HTTP request pipeline ---
        if (app.Environment.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurants"));
        }

        app.UseSerilogRequestLogging();

        // Redirect all HTTP requests to HTTPS for security.
        app.UseHttpsRedirection();

        app.UseAuthentication();
        // Add authorization middleware (checks user access before hitting controllers).
        app.UseAuthorization();

        // Map controller endpoints so they can handle incoming HTTP requests.
        app.MapControllers();

        // Return the configured app so it can be run.
        return app;
    }
}
