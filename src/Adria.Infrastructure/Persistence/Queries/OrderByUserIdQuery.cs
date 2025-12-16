using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class OrderByUserIdQuery(
    IOrderRepository orderRepository) : IOrderByUserIdQuery
{
    public async Task<IReadOnlyCollection<OrderData?>> Fetch(Guid id)
    {
        var orders = await orderRepository.ByUserId(id);

        return orders
            .Select(o => new OrderData(o.OrderId, o.AdrianId, o.Date, o.TotalPrice))
            .Cast<OrderData?>()
            .ToList()
            .AsReadOnly();
    }
}