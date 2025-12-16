using System.Collections.ObjectModel;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Order;


public sealed record OrderWithSupplementsData(
    Guid OrderId,
    Guid AdrianId,
    ReadOnlyCollection<SupplementData> Supplements,
    DateTime Date,
    double TotalPrice
);

public sealed record SearchOrderByUserIdInput(Guid AdrianId);

public sealed class SearchOrderByUserId(
    IOrderByUserIdQuery orderByUserIdQuery,
    IOrderSupplementDetailsRepository supplementDetailsRepository,
    ISupplementRepository supplementRepository,
    ILogger<SearchOrderByUserId> logger)
    : IUseCase<SearchOrderByUserIdInput, Task<IReadOnlyCollection<OrderWithSupplementsData>>>
{
    public async Task<IReadOnlyCollection<OrderWithSupplementsData>> Execute(SearchOrderByUserIdInput input)
    {
        var orders = await orderByUserIdQuery.Fetch(input.AdrianId);

        var result = new List<OrderWithSupplementsData>();

        foreach (var order in orders)
        {
            var supplements = await supplementDetailsRepository.ByOrderId(order.OrderId);

            result.Add(
                new OrderWithSupplementsData(
                    order.OrderId,
                    order.AdrianId,
                    supplements
                        .Select(s =>
                        {
                            var supplement = supplementRepository.ById(s.SupplementId).Result;
                            if (supplement != null)
                                return new SupplementData(
                                    supplement.SupplementId,
                                    supplement.Name,
                                    supplement.Type,
                                    supplement.Price,
                                    supplement.Stock
                                );
                            throw new ElementNotFoundException($"Supplement with id {s.SupplementId} not found");
                        })
                        .ToList()
                        .AsReadOnly(),
                    order.Date,
                    order.TotalPrice
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