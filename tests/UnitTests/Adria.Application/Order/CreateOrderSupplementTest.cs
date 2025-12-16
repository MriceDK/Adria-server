using Adria.Application.OrderSupplement;
using Adria.Domain.Order;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application.Order;

public sealed class CreateOrderSupplementTest
{
    [Fact]
    public async Task Execute_WithValidInput_SavesOrderSupplementAndReturnsEntity()
    {
        var repository = new MockOrderSupplementDetailsRepository();
        var logger = new MockLogger<CreateOrderSupplementDetails>();
        var useCase = new CreateOrderSupplementDetails(repository, logger);

        var orderId = Guid.NewGuid();
        var supplementId = Guid.NewGuid();
        var amount = 2;

        var input = new CreateOrderSupplementDetailsInput(
            orderId,
            supplementId,
            amount
        );

        var result = await useCase.Execute(input);

        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(supplementId, result.SupplementId);
        Assert.Equal(amount, result.Amount);

        Assert.Single(repository.SavedEntities);
        Assert.Equal(result, repository.SavedEntities[0]);

        Assert.Single(logger.LoggedMessages);
        Assert.Contains("New OrderSupplement created", logger.LoggedMessages[0]);
    }
}