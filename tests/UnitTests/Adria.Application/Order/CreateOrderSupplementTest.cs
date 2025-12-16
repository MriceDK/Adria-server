using Adria.Application.OrderSupplement;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Order;

public sealed class CreateOrderSupplementTest
{
    [Fact]
    public async Task Execute_WithValidInput_GeneratesOrderIdAndSavesOrderAndSupplements()
    {
        var orderRepository = new MockOrderRepository();
        var supplementRepository = new MockOrderSupplementDetailsRepository();
        var priceCalculator = new MockCalculateOrderTotalPrice();
        var logger = new MockLogger<CreateOrderSupplement>();

        var useCase = new CreateOrderSupplement(
            orderRepository,
            supplementRepository,
            priceCalculator,
            logger
        );

        var supplementId1 = Guid.NewGuid();
        var supplementId2 = Guid.NewGuid();

        var input = new CreateOrderInput(
            Guid.NewGuid(),
            new[]
            {
                new CreateOrderSupplementDetailItem(supplementId1, 2),
                new CreateOrderSupplementDetailItem(supplementId2, 5)
            }
        );

        var orderId = await useCase.Execute(input);

        Assert.NotEqual(Guid.Empty, orderId);

        Assert.Single(orderRepository.SavedOrders);
        Assert.Equal(orderId, orderRepository.SavedOrders[0].OrderId);

        Assert.Equal(2, supplementRepository.SavedEntities.Count);
        Assert.All(supplementRepository.SavedEntities, s => Assert.Equal(orderId, s.OrderId));

        Assert.Contains(
            supplementRepository.SavedEntities,
            s => s.SupplementId == supplementId1 && s.Amount == 2
        );

        Assert.Contains(
            supplementRepository.SavedEntities,
            s => s.SupplementId == supplementId2 && s.Amount == 5
        );
    }
}