using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;


public sealed record OrderWithSupplementsData(
    Guid OrderId,
    Guid AdrianId,
    DateTime Date,
    double TotalPrice,
    IReadOnlyCollection<OrderSupplementDetailsData> Supplements
);

public sealed record SearchOrderByUserIdInput(Guid AdrianId);

public sealed class SearchOrderByUserId(
    IOrderByUserIdQuery orderByUserIdQuery,
    IOrderSupplementDetailsRepository supplementDetailsRepository,
    ILogger<SearchOrderByUserId> logger)
    : IUseCase<SearchOrderByUserIdInput, Task<IReadOnlyCollection<OrderWithSupplementsData>>>
{
    public async Task<IReadOnlyCollection<OrderWithSupplementsData>> Execute(SearchOrderByUserIdInput input)
    {
        var orders = await orderByUserIdQuery.Fetch(input.AdrianId);

        var result = new List<OrderWithSupplementsData>();

        foreach (var order in orders.Where(o => o != null))
        {
            var supplements = await supplementDetailsRepository.ByOrderId(order!.OrderId);

            result.Add(
                new OrderWithSupplementsData(
                    order.OrderId,
                    order.AdrianId,
                    order.Date,
                    order.TotalPrice,
                    supplements
                        .Select(s => new OrderSupplementDetailsData(
                            s.OrderId,
                            s.SupplementId,
                            s.Amount
                        ))
                        .ToList()
                        .AsReadOnly()
                )
            );
        }

        logger.LogInformation(
            "Found {OrderCount} order(s) for user ID {AdrianId}",
            result.Count,
            input.AdrianId
        );

        return result.AsReadOnly();
    }

}