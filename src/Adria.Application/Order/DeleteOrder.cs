using Adria.Application.Contracts;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;

public sealed record DeleteOrderInput(
    Guid OrderId
);

public sealed class DeleteOrder(
    IOrderRepository orderRepository,
    ILogger<DeleteOrder> logger)
    : IUseCase<DeleteOrderInput, Task>
{
    public async Task Execute(DeleteOrderInput input)
    {
        Domain.Order.Order order = await orderRepository.ByOrderId(input.OrderId);
        
        await orderRepository.Remove(order);

        logger.LogInformation(
            "Deleted order with ID {OrderId}",
            input.OrderId
        );
    }
}
