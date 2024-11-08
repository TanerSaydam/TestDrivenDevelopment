using Testcontainers.PostgreSql;

namespace Pricing.Infrastructure.Tests;
public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer
        = new PostgreSqlBuilder().Build();

    public IDbConnectionFactory ConnectionFactory;
    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        ConnectionFactory = new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString());

        await new DatabaseInitializer(ConnectionFactory).InitializeAsync();
    }
}
