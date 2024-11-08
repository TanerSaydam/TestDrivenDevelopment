using FluentAssertions;
using Pricing.Api;
using Pricing.Core;
using System.Net.Http.Json;

namespace Pricing.Acceptance.Tests;

public class TickerPriceFeature : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public TickerPriceFeature(ApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
    }

    [Fact]
    public async Task GWT_30min_ticket_When_1hour_has_price_2_Then_return_2()
    {
        await _client.PutAsJsonAsync("PricingTable",
            new ApplyPricingRequest(new[]
            {
                new PriceTierRequest(1,2m),
                new PriceTierRequest(24,5)
            }.ToList()));
        var exit = DateTimeOffset.UtcNow;
        var entry = exit.AddMinutes(-30);

        var response =
            await _client.GetFromJsonAsync<TicketPriceResponse>(
                $"TicketPrice?entry={entry:u}&exit={exit:u}");

        response.Should().NotBeNull();
        response?.Price.Should().Be(2);
    }
}
