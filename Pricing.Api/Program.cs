using Pricing.Core;
using Pricing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(builder.Configuration.GetValue<string>("Database")));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddScoped<IPricingStore, PostgresPricingStore>();
builder.Services.AddScoped<IPricingManager, PricingManager>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPut("/PricingTable", async (IPricingManager piricingManager, ApplyPricingRequest request) =>
{
    try
    {
        var result = await piricingManager.HandleAsync(request, default);
        return result ? Results.Ok() : Results.BadRequest();
    }
    catch (InvalidPricingTierException)
    {
        return Results.Problem();
    }
});

await InitializeDatabase(app);


app.Run();

Task InitializeDatabase(WebApplication app) => app.Services
    .GetService<DatabaseInitializer>()?
    .InitializeAsync() ?? Task.CompletedTask;