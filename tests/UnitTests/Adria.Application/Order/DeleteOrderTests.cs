using Adria.Application.Order;
using Adria.Domain.Order;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Order;

public sealed class DeleteOrderTests
{
    [Fact]
    public async Task Execute_WithExistingOrder_RemovesOrderAndLogsSuccess()
    {
        var repository = new MockOrderRepository();
        var logger = new MockLogger<DeleteOrder>();
        var useCase = new DeleteOrder(repository, logger);

        var orderId = Guid.NewGuid();
        repository.Seed(new global::Adria.Domain.Order.Order(orderId, Guid.NewGuid(), DateTime.UtcNow, 0.0));

        var input = new DeleteOrderInput(orderId);

        await useCase.Execute(input);

        Assert.Empty(repository.Entities);
        Assert.Single(logger.LoggedMessages);
        Assert.Contains("Order removed", logger.LoggedMessages[0]);
    }
}