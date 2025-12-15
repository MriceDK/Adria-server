using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using System;
using System.Threading.Tasks;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class OrderByIdQuery(
    IOrderRepository orderRepository) : IOrderByIdQuery
{
    public async Task<OrderData?> Fetch(Guid id)
    {
        var order = await orderRepository.ById(id);

        if (order == null) return null;

        return new OrderData(order.OrderId, order.AdrianId, order.Date, order.TotalPrice);
    }
}