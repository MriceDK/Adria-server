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
    public async Task<IReadOnlyCollection<OrderData>?> Fetch(Guid adrianId)
    {
        var orders = await orderRepository.ByUserId(adrianId);

        if (!orders.Any()) return null;

        return orders
            .Select(o => new OrderData(o.OrderId, o.AdrianId, o.Date, o.TotalPrice))
            .ToList()
            .AsReadOnly();
    }
}