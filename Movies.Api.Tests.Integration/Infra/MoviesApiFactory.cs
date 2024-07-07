using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Movies.Api.Tests.Integration.Infra;

public class MoviesApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly DbFactory _dbFactory;

    public MoviesApiFactory()
    {
        _dbFactory = new DbFactory();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
        
        // This work with migrations
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Remove the previous configuration options
            config.Sources.Clear();
            
            // Alternate tests configurations
            var testConfig = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Movies"] = _dbFactory.DbConnection.ConnectionString,
                ["Jwt:Key"] = "SaltStoreAndLoadThisSecretSecurelyPepper",
                ["Jwt:Issuer"] = "https://identity.ces.com",
                ["Jwt:Audience"] = "https://movies.ces.com"
            };
            
            config.AddInMemoryCollection(testConfig);
        });

        // This work without migrations
        // builder.ConfigureServices(services =>
        // {
        //     services.RemoveAll(typeof(MoviesDbContext));
        //     services.AddDbContext<MoviesDbContext>(options => 
        //         options.UseNpgsql(_dbFactory.DbConnection.ConnectionString));
        // });
    }

    public async Task InitializeAsync()
    {
        await _dbFactory.InitializeAsync();
    }

    public new async  Task DisposeAsync()
    {
        await _dbFactory.DisposeAsync();
    }
}