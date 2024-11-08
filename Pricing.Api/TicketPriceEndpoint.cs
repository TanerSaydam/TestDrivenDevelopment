﻿using Microsoft.AspNetCore.Http.HttpResults;
using Pricing.Api.Tests;

namespace Pricing.Api;

public class TicketPriceEndpoint
{
    public static async Task<Ok<TicketPriceResponse>> HandleAsync(
        DateTimeOffset entry,
        DateTimeOffset exit,
        ITicketPriceService ticketPriceService,
        CancellationToken cancellationToken)
    {
        var result = await ticketPriceService.HandleAsync(new TicketPriceRequest(entry, exit), cancellationToken);

        return TypedResults.Ok<TicketPriceResponse>(result);
    }
}
