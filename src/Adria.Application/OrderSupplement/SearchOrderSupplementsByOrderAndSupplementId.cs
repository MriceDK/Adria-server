using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.OrderSupplement;

public sealed record SearchOrderSupplementsByOrderAndSupplementIdInput(Guid OrderId, Guid SupplementId);

public sealed class SearchOrderSupplementsByOrderAndSupplementId(
    IOrderSupplementDetailsRepository orderSupplementDetailsRepository,
    ILogger<SearchOrderSupplementsByOrderAndSupplementId> logger)
    : IUseCase<SearchOrderSupplementsByOrderAndSupplementIdInput, Task<OrderSupplementDetails>>
{
    public async Task<OrderSupplementDetails> Execute(SearchOrderSupplementsByOrderAndSupplementIdInput input)
    {
        logger.LogInformation(
            "Fetching order supplement details with order id '{OrderId}' and supplement id '{SupplementId}'",
            input.OrderId,
            input.SupplementId
        );

        return (await orderSupplementDetailsRepository.ByOrderAndSupplementId(input.OrderId, input.SupplementId))
               ?? throw new ElementNotFoundException(
                   $"OrderSupplementDetails with order id {input.OrderId} and supplement id {input.SupplementId} not found."
               );
    }
}