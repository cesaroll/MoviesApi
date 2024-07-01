using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.App.Database;
using Movies.App.Services;

namespace Movies.App;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMoviesService, MoviesService>();
        return services;
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<MoviesDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Movies")!));
        return services;
    }
    
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
        await context.Database.MigrateAsync();
    }
}