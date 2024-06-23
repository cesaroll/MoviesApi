using System.Data.Common;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using FluentAssertions;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Movies.Api.Tests.Integration;

public sealed class DbFactory : IAsyncLifetime
{
    private readonly INetwork _network = new NetworkBuilder().Build();

    private readonly IContainer _dbContainer;

    public DbFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithNetwork(_network)
            .WithNetworkAliases(nameof(_dbContainer))
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilContainerIsHealthy())
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilMessageIsLogged(
                    "PostgreSQL init process complete", 
                    ws => ws.WithTimeout(TimeSpan.FromSeconds(30))))
            .Build();
    }
    
    public DbConnection DbConnection => new NpgsqlConnection(((PostgreSqlContainer)_dbContainer).GetConnectionString());
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}