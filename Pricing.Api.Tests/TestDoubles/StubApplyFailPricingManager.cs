﻿using Pricing.Core;

namespace Pricing.Api.Tests.TestDoubles;

internal class StubApplyFailPricingManager : IPricingManager
{
    public Task<bool> HandleAsync(ApplyPricingRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(false);
    }
}