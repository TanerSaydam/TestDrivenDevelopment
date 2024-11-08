namespace Pricing.Api;

public sealed class TicketPriceResponse
{
    public TicketPriceResponse(decimal price)
    {
        Price = price;
    }
    public decimal Price { get; set; }
}