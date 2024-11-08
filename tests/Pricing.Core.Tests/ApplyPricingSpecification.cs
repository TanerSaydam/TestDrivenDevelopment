
using FluentAssertions;
using NSubstitute;
using Pricing.Core.Tests.TestDoubles;

namespace Pricing.Core.Tests;
public class ApplyPricingSpecification : IClassFixture<FakeFixture>, IDisposable
{

    private static readonly int _maxHourLimit = 24;
    private static readonly decimal _exptectedPrice = 1;

    [Fact]
    public async Task Should_throw_argument_null_exception_if_request_is_null()
    {
        var pricingManager = new PricingManager(new DummyPricingStore());

        var handleRequest = () => pricingManager.HandleAsync(null!, default);

        await handleRequest.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task Should_return_true_if_succeeded()
    {
        var pricingManager = new PricingManager(new StubSuccessPricingStore());

        var result = await pricingManager.HandleAsync(CreateRequest(), default);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_return_fail_if_fails_to_save()
    {
        var pricingManager = new PricingManager(new StubFailPricingStore());

        var result = await pricingManager.HandleAsync(CreateRequest(), default);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task Should_save_only_one()
    {
        var spyPricingService = new SpyPricingService();

        var pricingManager = new PricingManager(spyPricingService);

        _ = await pricingManager.HandleAsync(CreateRequest(), default);

        spyPricingService.NumberOfSaves.Should().Be(1);
    }

    [Fact]
    public async Task Should_save_expected_data()
    {
        var expectedPricingTable = new PricingTable(new[] { new PriceTier(hourLimit: _maxHourLimit, price: _exptectedPrice) });
        var mockPriceStore = new MockPricingStore();
        mockPriceStore.ExpectedToSave(expectedPricingTable);
        var pricingManager = new PricingManager(mockPriceStore);

        _ = await pricingManager.HandleAsync(CreateRequest(), default);

        mockPriceStore.Verify();
    }

    [Fact]
    public async Task Should_save_expected_data_nsubstitute()
    {
        var expectedPricingTable = new PricingTable(new[] { new PriceTier(hourLimit: _maxHourLimit, price: _exptectedPrice) });
        var mockPriceStore = Substitute.For<IPricingStore>();

        var pricingManager = new PricingManager(mockPriceStore);

        _ = await pricingManager.HandleAsync(CreateRequest(), default);

        await mockPriceStore.Received().SaveAsync(Arg.Is<PricingTable>(table => table.Tiers.Count == expectedPricingTable.Tiers.Count), default);
    }

    [Fact]
    public async void Should_save_equivalent_data_to_storage()
    {
        var pricingStore = new InMemoryPricingStore();
        var pricingManager = new PricingManager(pricingStore);
        var applyPricingRequest = CreateRequest();

        _ = await pricingManager.HandleAsync(applyPricingRequest, default);

        pricingStore
            .GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);
    }

    private readonly PricingManager _pricingManager;
    private readonly InMemoryPricingStore _pricingStore;
    public ApplyPricingSpecification()
    {
        _pricingStore = new InMemoryPricingStore();
        _pricingManager = new PricingManager(_pricingStore);
    }

    [Fact]
    public async void Lifecycle()
    {

        var applyPricingRequest = CreateRequest();

        _ = await _pricingManager.HandleAsync(applyPricingRequest, default);

        _pricingStore
            .GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);
    }

    [Fact]
    public async void Lifecycle2()
    {

        var applyPricingRequest = CreateRequest();

        _ = await _pricingManager.HandleAsync(applyPricingRequest, default);

        _pricingStore
            .GetPricingTable()
            .Should()
            .BeEquivalentTo(applyPricingRequest);
    }

    private static ApplyPricingRequest CreateRequest()
    {
        return new ApplyPricingRequest(new() { new PriceTierRequest(_maxHourLimit, _exptectedPrice) });
    }

    public void Dispose()
    {
        _pricingStore.Clean();
    }
}
