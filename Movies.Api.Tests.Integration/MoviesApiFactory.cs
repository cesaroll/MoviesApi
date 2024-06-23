using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Movies.Api.Tests.Integration;

public class MoviesApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly DbFactory _dbFactory;

    public MoviesApiFactory()
    {
        _dbFactory = new DbFactory();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Remove the previous configuration options
            config.Sources.Clear();
            
            // Alternate tests configurations
            var testConfig = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Movies"] = _dbFactory.DbConnection.ConnectionString
            };
            
            config.AddInMemoryCollection(testConfig);
        });
        
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
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