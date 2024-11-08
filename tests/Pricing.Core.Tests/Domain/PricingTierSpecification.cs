using FluentAssertions;

namespace Pricing.Core.Tests.Domain;
public sealed class PricingTierSpecification
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(25)]
    public void Should_throw_invalid_pricing_tier_exception_when_hour_limit_is_invalid(int hourLimit)
    {
        var create = () => new PriceTier(hourLimit: hourLimit, price: 1);

        create.Should().ThrowExactly<InvalidPricingTierException>();
    }

    [Fact]
    public void Should_throw_invalid_pricing_tier_exception_when_hour_price_is_negative()
    {
        var create = () => new PriceTier(hourLimit: 1, price: -1);

        create.Should().ThrowExactly<InvalidPricingTierException>();
    }
}
