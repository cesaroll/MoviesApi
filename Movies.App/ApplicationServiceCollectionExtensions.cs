using Microsoft.Extensions.DependencyInjection;
using Movies.App.Repositories;

namespace Movies.App;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
}