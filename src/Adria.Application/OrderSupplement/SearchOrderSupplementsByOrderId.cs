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
    : IUseCase<SearchOrderSupplementsByOrderIdInput, Task<IReadOnlyCollection<OrderSupplementDetailsData>>>
{
    public async Task<IReadOnlyCollection<OrderSupplementDetailsData>> Execute(SearchOrderSupplementsByOrderIdInput input)
    {
            logger.LogInformation(
                "Fetching order supplement details with order id '{OrderId}'.",
                input.OrderId
            );

            var entities = await orderSupplementDetailsRepository.ByOrderId(input.OrderId);

            return entities.Select(e => new OrderSupplementDetailsData(
                e.OrderId,
                e.SupplementId,
                e.Amount
            )).ToList().AsReadOnly();
        }
}