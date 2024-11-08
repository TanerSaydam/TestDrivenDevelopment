namespace Pricing.Core.Tests.TestDoubles;

public sealed class DummyPricingStore : IPricingStore
{
    public Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}