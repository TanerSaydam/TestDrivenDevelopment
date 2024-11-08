namespace Pricing.Api.Tests;

public class TicketPriceRequest
{
    public DateTimeOffset entry;
    public DateTimeOffset exit;

    public TicketPriceRequest(DateTimeOffset entry, DateTimeOffset exit)
    {
        this.entry = entry;
        this.exit = exit;
    }
}