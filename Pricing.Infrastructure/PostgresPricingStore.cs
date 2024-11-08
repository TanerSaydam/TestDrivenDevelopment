using Dapper;
using Pricing.Core;
using System.Text.Json;

namespace Pricing.Infrastructure;

public sealed class PostgresPricingStore : IPricingStore
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PostgresPricingStore(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
        ArgumentNullException.ThrowIfNull(dbConnectionFactory);
    }

    public async Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        var data = new DbRecord(JsonSerializer.Serialize(request));
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var result = await connection.ExecuteAsync(
            @"INSERT INTO Pricing (key, document) VALUES (@key, @document)
            ON CONFLICT (key) DO UPDATE
                SET document = excluded.document;",
            data
            );

        return result > 0;
    }

    private record DbRecord(string Document, string Key = "ACTIVE");
}