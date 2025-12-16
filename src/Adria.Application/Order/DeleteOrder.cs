using Adria.Application.Contracts;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;

public sealed record DeleteOrderInput(
    Guid OrderId
);

public sealed class DeleteOrder(
    IOrderRepository repository,
    ILogger<DeleteOrder> logger
)
    : IUseCase<DeleteOrderInput, Task>
{
    public async Task Execute(DeleteOrderInput input)
    {
        var order = await repository.ById(input.OrderId);

        if (order is null)
        {
            throw new ElementNotFoundException(
                $"Order with ID {input.OrderId} not found."
            );
        }

        await repository.Remove(order);

        logger.LogInformation("Order removed");
    }
}