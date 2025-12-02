using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.OrderSupplement;

public sealed record SearchOrderSupplementsBySupplementIdInput(Guid SupplementId);

public sealed class SearchOrderSupplementsBySupplementId(
    IOrderSupplementDetailsRepository orderSupplementDetailsRepository,
    ILogger<SearchOrderSupplementsBySupplementId> logger)
    : IUseCase<SearchOrderSupplementsBySupplementIdInput, Task<IReadOnlyCollection<OrderSupplementDetails>>>
{
    public async Task<IReadOnlyCollection<OrderSupplementDetails>> Execute(SearchOrderSupplementsBySupplementIdInput input)
    {
        logger.LogInformation(
            "Fetching order supplement details with order id '{OrderId}'.",
            input.SupplementId
        );

        return (await orderSupplementDetailsRepository.BySupplementId(input.SupplementId))
               ?? throw new ElementNotFoundException(
                   $"OrderSupplementDetails with supplement id {input.SupplementId} not found."
               );
    }
}