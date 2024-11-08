
namespace Pricing.Core;

public sealed record ApplyPricingRequest(
    List<PriceTierRequest> Tiers);

public sealed record PriceTierRequest(int HourLimit, decimal Price);