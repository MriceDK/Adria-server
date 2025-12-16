using Adria.Application.Contracts;
using Adria.Domain.Order;
using Microsoft.Extensions.Logging;

namespace Adria.Application.OrderSupplement;

public sealed record CreateOrderSupplementDetailItem(
    Guid SupplementId,
    int Amount
);

public sealed record CreateOrderInput(
    Guid AdrianId,
    IReadOnlyCollection<CreateOrderSupplementDetailItem> Supplements
);


public sealed class CreateOrderSupplement(
    IOrderRepository orderRepository,
    IOrderSupplementDetailsRepository orderSupplementRepository,
    IUseCase<IReadOnlyCollection<CreateOrderSupplementDetailItem>, Task<double>> calculateTotalPrice,
    ILogger<CreateOrderSupplement> logger
) : IUseCase<CreateOrderInput, Task<Guid>>
{
    public async Task<Guid> Execute(CreateOrderInput input)
    {
        var orderId = Guid.NewGuid();
        var date = DateTime.UtcNow;

        var totalPrice = await calculateTotalPrice.Execute(input.Supplements);

        var order = new Domain.Order.Order(
            orderId,
            input.AdrianId,
            date,
            totalPrice
        );

        await orderRepository.Save(order);

        var supplements = input.Supplements
            .GroupBy(s => s.SupplementId)
            .Select(g => new OrderSupplementDetails(
                orderId,
                g.Key,
                g.Sum(x => x.Amount)
            ))
            .ToList();

        await orderSupplementRepository.SaveMany(supplements);

        logger.LogInformation(
            "Created order {OrderId} for AdrianId {AdrianId} with total price {TotalPrice}",
            orderId,
            input.AdrianId,
            totalPrice
        );

        return orderId;
    }
}