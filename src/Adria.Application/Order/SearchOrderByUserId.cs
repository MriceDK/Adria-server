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
    : IUseCase<SearchOrderByUserIdInput, Task<IReadOnlyCollection<OrderData>>> 
{
    public async Task<IReadOnlyCollection<OrderData>> Execute(SearchOrderByUserIdInput input)
    {
        var orders = await orderByUserIdQuery.Fetch(input.AdrianId);

        var nonNullOrders = orders
            .Where(o => o is not null)
            .Select(o => o!)
            .ToArray();

        logger.LogInformation(
            "Found order(s) for user ID {AdrianId}",
            input.AdrianId
        );

        return nonNullOrders;
    }
}