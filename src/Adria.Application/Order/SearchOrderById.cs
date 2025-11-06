using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;

public sealed record SearchOrderByIdInput(
    Guid OrderId
);

public sealed class SearchOrderById : IUseCase<SearchOrderByIdInput, Task<OrderData>>
{
    private readonly IOrderByIdQuery _orderByIdQuery;
    private readonly ILogger<SearchOrderById> _logger;

    public SearchOrderById(
        IOrderByIdQuery orderByIdQuery,
        ILogger<SearchOrderById> logger
    )
    {
        _orderByIdQuery = orderByIdQuery;
        _logger = logger;
    }

    public async Task<OrderData> Execute(SearchOrderByIdInput input)
    {
        _logger.LogInformation(
            "Fetching order with ID {OrderId}",
            input.OrderId
        );

        return (await _orderByIdQuery.Fetch(input.OrderId))
               ?? throw new ElementNotFoundException(
                   $"Order with ID {input.OrderId} not found."
               );
    }
}