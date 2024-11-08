using System.Collections.Immutable;

namespace Pricing.Core;

public sealed class PricingTable
{
    public PricingTable(IEnumerable<PriceTier> tiers, decimal? maxDailyPrice = null)
    {
        _maxDailyPrice = maxDailyPrice;
        Tiers = tiers?.OrderBy(tier => tier.HourLimit).ToImmutableArray() ?? throw new ArgumentNullException();

        if (!Tiers.Any())
        {
            throw new ArgumentException(message: "Mising Pricing Tiers", nameof(Tiers));
        }

        if (Tiers.Last().HourLimit < 24)
        {
            throw new ArgumentException();
        }

        if (_maxDailyPrice.HasValue && _maxDailyPrice.Value > CalculateMaxDailyPriceFromTiers())
        {
            throw new ArgumentException();
        }
    }

    public IReadOnlyCollection<PriceTier> Tiers { get; internal set; }
    public decimal? _maxDailyPrice { get; }

    public decimal GetMaxDailyPrice()
    {
        if (_maxDailyPrice.HasValue)
        {
            return _maxDailyPrice.Value;
        }

        decimal total = CalculateMaxDailyPriceFromTiers();
        return total;
    }

    private decimal CalculateMaxDailyPriceFromTiers()
    {
        decimal total = 0;
        var hoursIncluded = 0;
        foreach (var tier in Tiers)
        {
            total += tier.Price * (tier.HourLimit - hoursIncluded);
            hoursIncluded = tier.HourLimit;
        }

        return total;
    }
}