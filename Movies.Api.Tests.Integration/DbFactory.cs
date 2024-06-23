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

    private readonly IContainer _postgreSqlContainer;

    public DbFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithNetwork(_network)
            .WithNetworkAliases(nameof(_postgreSqlContainer))
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilContainerIsHealthy())
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilMessageIsLogged(
                    "PostgreSQL init process complete", 
                    ws => ws.WithTimeout(TimeSpan.FromSeconds(30))))
            .Build();
    }
    
    public DbConnection DbConnection => new NpgsqlConnection(((PostgreSqlContainer)_postgreSqlContainer).GetConnectionString());
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}