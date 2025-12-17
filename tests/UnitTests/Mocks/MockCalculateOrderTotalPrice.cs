using Adria.Application.Contracts;
using Adria.Application.OrderSupplement;

namespace UnitTests.Mocks;

public sealed class MockCalculateOrderTotalPrice
    : IUseCase<IReadOnlyCollection<CreateOrderSupplementDetailItem>, Task<double>>
{
    public Task<double> Execute(IReadOnlyCollection<CreateOrderSupplementDetailItem> items)
    {
        return Task.FromResult(100.0);
    }
}