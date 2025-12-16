using Adria.Application.OrderSupplement;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using System;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application.Order;

public sealed class DeleteOrderSupplementTest
{
    [Fact]
    public async Task Execute_WithExistingOrderSupplement_RemovesAndReturnsEntity()
    {
        var repository = new MockOrderSupplementDetailsRepository();
        var logger = new MockLogger<DeleteOrderSupplementDetails>();
        var useCase = new DeleteOrderSupplementDetails(repository, logger);

        var orderId = Guid.NewGuid();
        var supplementId = Guid.NewGuid();

        var existing = new OrderSupplementDetails(orderId, supplementId, 3);
        repository.Seed(existing);

        var input = new DeleteOrderSupplementDetailsInput(orderId, supplementId);

        var result = await useCase.Execute(input);

        Assert.Equal(existing, result);
        Assert.Empty(repository.Entities);
        Assert.Single(logger.LoggedMessages);
        Assert.Contains("OrderSupplement removed", logger.LoggedMessages[0]);
    }
}