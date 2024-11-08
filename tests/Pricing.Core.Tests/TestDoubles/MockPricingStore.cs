

using FluentAssertions;

namespace Pricing.Core.Tests.TestDoubles;

public sealed class MockPricingStore : IPricingStore
{
    private PricingTable _expectedPricingTable;
    private PricingTable _savedPricingTable;
    public Task<bool> SaveAsync(PricingTable request, CancellationToken cancellationToken)
    {
        _savedPricingTable = request;
        return Task.FromResult(true);
    }

    public void ExpectedToSave(PricingTable expectedPricingTable)
    {
        _expectedPricingTable = expectedPricingTable;
    }

    public void Verify()
    {
        _savedPricingTable.Should().BeEquivalentTo(_expectedPricingTable);
    }
}