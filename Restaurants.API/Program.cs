using Restaurants.API.Extensions;
using Restaurants.Infrastructure.Seeders;

public class Program {
    public static async Task Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        var app = await builder.ConfigureServices().ConfigurePipeline();

        await app.RunAsync();
    }
}
