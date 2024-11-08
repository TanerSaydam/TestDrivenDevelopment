using FluentAssertions;

namespace Pricing.Core.Tests.Domain;

public class PricingTableSpecification
{
    private const int maxDailyPrice = 15;

    [Fact]
    public void Should_trow_argument_null_exception_if_price_tries_is_null()
    {
        Func<PricingTable> create = () => new PricingTable(null!);

        create.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_throw_agument_exception_if_has_no_price_tiers()
    {
        var create = () => new PricingTable(Array.Empty<PriceTier>());

        create.Should()
            .ThrowExactly<ArgumentException>()
            .WithMessage("Mising Pricing Tiers*")
            .WithParameterName(nameof(PricingTable.Tiers));
    }

    [Fact]
    public void Should_have_one_tier_when_created_with_one()
    {
        var pricingTable = new PricingTable(new[] { CreatePriceTier() });

        pricingTable.Tiers.Should().HaveCount(1);
    }

    [Fact]
    public void Price_tiers_should_be_ordered_by_hour_limit()
    {
        var pricingTable = new PricingTable(new[]
        {
            CreatePriceTier(hourLimit: 4),
            CreatePriceTier(hourLimit: 24)
        });

        pricingTable.Tiers.Should().BeInAscendingOrder(tier => tier.HourLimit);

    }

    [Theory]
    [InlineData(2, 1, 28)]
    [InlineData(3, 2, 52)]
    public void Maximum_daily_price_should_be_calculated_using_tiers_if_not_defined(decimal price1, decimal price2, decimal maxPrice)
    {
        var pricingTable = new PricingTable(new[]
        {
            CreatePriceTier(hourLimit: 4, price: price1),
            CreatePriceTier(hourLimit: 24, price: price2)
        }, maxDailyPrice: null);

        pricingTable.GetMaxDailyPrice().Should().Be(maxPrice);
    }

    [Fact]
    public void Should_be_able_to_set_maximum_daily_price()
    {
        var pricingTable = new PricingTable(
            new[]
            {
                CreatePriceTier(hourLimit: 24, price: 1)
            }, maxDailyPrice: maxDailyPrice);

        pricingTable.GetMaxDailyPrice().Should().Be(maxDailyPrice);
    }

    [Fact]
    public void Should_throw_argument_exception_if_tiers_do_not_cover_24h()
    {
        var create = () => new PricingTable(new[]
        {
            CreatePriceTier(hourLimit: 20)
        });

        create.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Should_throw_argument_exception_if_max_daily_price_gt_tiers_price()
    {
        var create = () => new PricingTable(new[]
        {
            CreatePriceTier(hourLimit: 24, price:1)
        }, maxDailyPrice: 26);

        create.Should().Throw<ArgumentException>();
    }
    private static PriceTier CreatePriceTier(int hourLimit = 24, decimal price = 1) => new PriceTier(hourLimit, price);
}
