using Adria.Application.Contracts;
using Adria.Domain.Order;
using Microsoft.Extensions.Logging;

namespace Adria.Application.OrderSupplement;

public sealed record CreateOrderSupplementDetailsInput(
    Guid OrderId,
    Guid SupplementId,
    int Amount
);

public sealed class CreateOrderSupplementDetails(IOrderSupplementDetailsRepository orderSupplementDetailsRepository, ILogger<CreateOrderSupplementDetails> logger)
    : IUseCase<CreateOrderSupplementDetailsInput, Task<OrderSupplementDetails>>
{
    public async Task<OrderSupplementDetails> Execute(CreateOrderSupplementDetailsInput input)
    {
        OrderSupplementDetails orderSupplement = new(input.OrderId, input.SupplementId, input.Amount);

        await orderSupplementDetailsRepository.Save(orderSupplement);

        logger.LogInformation(
            "New OrderSupplement created with Order ID {OrderId}, Supplement ID: {SupplementId} and Amount: {Amount}",
            orderSupplement.OrderId,
            orderSupplement.SupplementId,
            orderSupplement.Amount
        );

        return orderSupplement;
    }
}