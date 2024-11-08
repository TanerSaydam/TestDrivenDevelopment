namespace Pricing.Core.Tests.TestDoubles;

public sealed class StubFailPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }
}