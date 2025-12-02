using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.OrderSupplement;

public sealed record SearchOrderSupplementsByIdInput(Guid OrderId);

public sealed class SearchOrderSupplementsById(
    IOrderSupplementDetailsRepository orderSupplementDetailsRepository,
    ILogger<SearchOrderSupplementsById> logger)
    : IUseCase<SearchOrderSupplementsByIdInput, Task<IReadOnlyCollection<OrderSupplementDetails>>>
{
    public async Task<IReadOnlyCollection<OrderSupplementDetails>> Execute(SearchOrderSupplementsByIdInput input)
    {
        logger.LogInformation(
            "Fetching order supplement details with order id '{OrderId}'.",
            input.OrderId
        );

        return (await orderSupplementDetailsRepository.ByOrderId(input.OrderId))
               ?? throw new ElementNotFoundException(
                   $"OrderSupplementDetails with id {input.OrderId} not found."
               );
    }
}