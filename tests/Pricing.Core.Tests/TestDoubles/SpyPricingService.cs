namespace Pricing.Core.Tests.TestDoubles;

public sealed class SpyPricingService : IPricingStore
{
    public int NumberOfSaves { get; private set; }

    public Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        NumberOfSaves++;

        return Task.FromResult(true);
    }
}