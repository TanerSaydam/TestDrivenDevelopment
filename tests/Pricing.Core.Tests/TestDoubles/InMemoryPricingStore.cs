
namespace Pricing.Core.Tests.TestDoubles;

public sealed class InMemoryPricingStore : IPricingStore
{
    private PricingTable _pricingTable;
    public Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        _pricingTable = request;
        return Task.FromResult(true);
    }

    public void Clean()
    {
        _pricingTable = null;
    }

    public PricingTable GetPricingTable()
    {
        return _pricingTable;
    }
}