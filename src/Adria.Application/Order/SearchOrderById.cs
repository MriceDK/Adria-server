using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;

public sealed record SearchOrderByIdInput(
    Guid OrderId
);

public sealed class SearchOrderById(
    IOrderByIdQuery orderByIdQuery,
    ILogger<SearchOrderById> logger)
    : IUseCase<SearchOrderByIdInput, Task<OrderData>>
{
    public async Task<OrderData> Execute(SearchOrderByIdInput input)
    {
        logger.LogInformation(
            "Fetching order with ID {OrderId}",
            input.OrderId
        );

        return (await orderByIdQuery.Fetch(input.OrderId))
               ?? throw new ElementNotFoundException(
                   $"Order with ID {input.OrderId} not found."
               );
    }
}