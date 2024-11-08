namespace Pricing.Core;

public interface IPricingStore
{
    Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken);
}