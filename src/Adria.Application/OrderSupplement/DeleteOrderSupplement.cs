using Adria.Application.Contracts;
using Adria.Domain.Order;
using Microsoft.Extensions.Logging;
using Adria.Domain.Shared.Exceptions;

namespace Adria.Application.OrderSupplement;

public sealed record DeleteOrderSupplementDetailsInput(
    Guid OrderId,
    Guid SupplementId
);

public sealed class DeleteOrderSupplementDetails(IOrderSupplementDetailsRepository orderSupplementDetailsRepository, ILogger<DeleteOrderSupplementDetails> logger)
    : IUseCase<DeleteOrderSupplementDetailsInput, Task<OrderSupplementDetails>>
{
    public async Task<OrderSupplementDetails> Execute(DeleteOrderSupplementDetailsInput input)
    {
        OrderSupplementDetails orderSupplement =
            (await orderSupplementDetailsRepository
                .ByOrderAndSupplementId(input.OrderId, input.SupplementId))!;
        
        if (orderSupplement is null)
        {
            throw new ElementNotFoundException($"Supplement with ID {input.SupplementId} not found.");
        }
        
        await orderSupplementDetailsRepository.Remove(orderSupplement);
        
        logger.LogInformation(
            "OrderSupplement removed for Order ID {OrderId}, Supplement ID: {SupplementId}",
            orderSupplement.OrderId,
            orderSupplement.SupplementId
        );

        return orderSupplement;
    }
}