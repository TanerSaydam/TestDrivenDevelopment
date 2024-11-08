
using Pricing.Api.Tests;

namespace Pricing.Api;

public interface ITicketPriceService
{
    Task<TicketPriceResponse> HandleAsync(TicketPriceRequest ticketPriceRequest, CancellationToken cancellationToken);
}