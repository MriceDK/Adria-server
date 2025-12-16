using Adria.Application.Order;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Order;

public sealed class CreateOrderTests
{
    [Fact]
    public async Task Execute_WithValidInput_CreatesOrderAndReturnsId()
    {
        var repository = new MockOrderRepository();
        var logger = new MockLogger<CreateOrder>();
        var useCase = new CreateOrder(repository, logger);

        var input = new CreateOrderInput(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow,
            0.0
        );

        var result = await useCase.Execute(input);

        Assert.NotEqual(Guid.Empty, result);
        Assert.Single(repository.SavedOrders);
        Assert.Equal(result, repository.SavedOrders[0].OrderId);
    }
}