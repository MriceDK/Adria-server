using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;

public sealed record SearchOrderByUserIdInput(
    Guid AdrianId
);

public sealed class SearchOrderByUserId(
    IOrderByUserIdQuery orderByUserIdQuery,
    ILogger<SearchOrderByUserId> logger)
    : IUseCase<SearchOrderByUserIdInput, Task<OrderData>>
{
    public async Task<OrderData> Execute(SearchOrderByUserIdInput input)
    {
        logger.LogInformation(
            "Fetching orders with user ID {AdrianId}",
            input.AdrianId
        );

        return (await orderByUserIdQuery.Fetch(input.AdrianId))
               ?? throw new ElementNotFoundException(
                   $"Order with ID {input.AdrianId} not found."
               );
    }
}