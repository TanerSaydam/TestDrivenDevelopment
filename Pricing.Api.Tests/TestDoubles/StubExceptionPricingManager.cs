using Pricing.Core;

namespace Pricing.Api.Tests.TestDoubles;

public class StubExceptionPricingManager : IPricingManager
{
    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        throw new InvalidPricingTierException();
    }
}