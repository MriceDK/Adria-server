using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.OrderSupplement;

public sealed record SearchOrderSupplementsByOrderIdInput(Guid OrderId);

public sealed class SearchOrderSupplementsByOrderId(
    IOrderSupplementDetailsRepository orderSupplementDetailsRepository,
    ILogger<SearchOrderSupplementsByOrderId> logger)
    : IUseCase<SearchOrderSupplementsByOrderIdInput, Task<IReadOnlyCollection<OrderSupplementDetails>>>
{
    public async Task<IReadOnlyCollection<OrderSupplementDetails>> Execute(SearchOrderSupplementsByOrderIdInput input)
    {
        logger.LogInformation(
            "Fetching order supplement details with order id '{OrderId}'.",
            input.OrderId
        );

        return (await orderSupplementDetailsRepository.ByOrderId(input.OrderId))
               ?? throw new ElementNotFoundException(
                   $"OrderSupplementDetails with order id {input.OrderId} not found."
               );
    }
}