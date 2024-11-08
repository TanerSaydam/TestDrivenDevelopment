using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pricing.Api.Tests.TestDoubles;
using Pricing.Core;
using Pricing.Core.Tests.TestDoubles;
using System.Net;
using System.Net.Http.Json;

namespace Pricing.Api.Tests;

public class ApplyPricingEndpointSpecification
{
    private const string _requestUri = "PricingTable";

    [Fact]
    public async Task Should_return_500_when_causes_exception()
    {
        var api = CreateApiWithPricingManager<StubExceptionPricingManager>();
        using var client = api.CreateClient();
        var response = await client.PutAsJsonAsync(_requestUri, CreateRequest());

        response.Should().HaveStatusCode(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Should_return_400_when_pricing_manager_return_false()
    {
        var api = CreateApiWithPricingManager<StubApplyFailPricingManager>();
        using var client = api.CreateClient();

        var response = await client.PutAsJsonAsync(_requestUri, CreateRequest());

        response.Should().HaveStatusCode(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_return_200_when_pricing_manager_return_false()
    {
        var api = CreateApiWithPricingManager<StubApplySucceedPricingManager>();
        using var client = api.CreateClient();

        var response = await client.PutAsJsonAsync(_requestUri, CreateRequest());

        response.Should().HaveStatusCode(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_send_request_to_pricing_manager()
    {
        var pricingStore = new InMemoryPricingStore();
        var api = new ApiFactory(services =>
        {
            services.TryAddScoped<IPricingStore>(s => pricingStore);
        });
        var applyPricingRequest = CreateRequest();

        var response = await api.CreateClient().PutAsJsonAsync(_requestUri, applyPricingRequest);

        pricingStore.GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);
    }

    private static ApplyPricingRequest CreateRequest()
    {
        return new ApplyPricingRequest(new() { new PriceTierRequest(24, 1) });
    }

    private static ApiFactory CreateApiWithPricingManager<T>()
        where T : class, IPricingManager
    {
        var api = new ApiFactory(services =>
        {
            services.RemoveAll(typeof(IPricingManager));

            services.TryAddScoped<IPricingManager, T>();
        });

        return api;
    }
}
