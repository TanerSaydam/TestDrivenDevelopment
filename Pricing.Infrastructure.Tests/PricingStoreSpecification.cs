using Dapper;
using FluentAssertions;
using Pricing.Core;
using System.Data;
using System.Text.Json;

namespace Pricing.Infrastructure.Tests;

public class PricingStoreSpecification : IClassFixture<PostgreSqlFixture>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public PricingStoreSpecification(PostgreSqlFixture fixture)
    {
        _dbConnectionFactory = fixture.ConnectionFactory;
    }

    [Fact]
    public void Should_throw_argument_null_exception_if_missing_connection_string()
    {
        var create = () => new PostgresPricingStore(null!);

        create.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task Should_return_true_when_save_with_success()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        PricingTable pricingTable = CreatePricingTable();

        var result = await store.SaveAsync(pricingTable, default);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_insert_if_not_exists()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        PricingTable pricingTable = CreatePricingTable();
        using IDbConnection connectin = await CleanUpPricingStore();

        var result = await store.SaveAsync(pricingTable, default);

        result.Should().BeTrue();
    }

    private async Task<IDbConnection> CleanUpPricingStore()
    {
        var connectin = await _dbConnectionFactory.CreateConnectionAsync();
        await connectin.ExecuteAsync("truncate table pricing");
        return connectin;
    }

    [Fact]
    public async Task Should_replace_if_already_exists()
    {
        IPricingStore store = new PostgresPricingStore(_dbConnectionFactory);
        var pricingTable = CreatePricingTable();
        await store.SaveAsync(pricingTable, default);
        var newPricingTable = CreatePricingTable();

        var result = await store.SaveAsync(newPricingTable, default);

        result.Should().BeTrue();

        var data = await GetPricingFromStore();
        data.Should()
            .HaveCount(1)
            .And
            .Subject
            .First().document.Equals(JsonSerializer.Serialize(newPricingTable));
    }

    private async Task<IEnumerable<dynamic>> GetPricingFromStore()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync(
                    @"SELECT * FROM pricing;");
    }

    private static PricingTable CreatePricingTable()
    {
        return new PricingTable(new[]
        {
            new PriceTier(hourLimit: 24, price: 0.5m)
        });
    }
}
