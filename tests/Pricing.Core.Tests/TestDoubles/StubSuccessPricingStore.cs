namespace Pricing.Core.Tests.TestDoubles;

public sealed class StubSuccessPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}