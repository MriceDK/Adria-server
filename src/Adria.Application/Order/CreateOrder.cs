using Adria.Application.Contracts;
using Adria.Domain.Order;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;

public sealed record CreateOrderInput(
    Guid OrderId,
    Guid AdrianId,
    DateTime Date,
    double TotalPrice
);

public sealed class CreateOrder : IUseCase<CreateOrderInput, Task<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CreateOrder> _logger;

    public CreateOrder(IOrderRepository orderRepository, ILogger<CreateOrder> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Guid> Execute(CreateOrderInput input)
    {
        Domain.Order.Order order = new(input.OrderId, input.AdrianId, input.Date, input.TotalPrice);

        await _orderRepository.Save(order);

        _logger.LogInformation(
            "New order created with ID {OrderId}, AdrianId: {AdrianId}, Date: {Date}, TotalPrice: {TotalPrice}",
            order.OrderId,
            order.AdrianId,
            order.Date,
            order.TotalPrice
        );

        return order.OrderId;
    }
}