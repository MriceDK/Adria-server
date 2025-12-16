using Adria.Application.Contracts;
using Adria.Domain.Order;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();
    public List<Order> SavedOrders { get; } = new();
    public IReadOnlyCollection<Order> Entities => _orders.Values.ToList().AsReadOnly();

    public void Seed(Order order)
    {
        _orders[order.OrderId] = order;
    }

    public Task<Order?> ById(Guid orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }

    public Task<IReadOnlyCollection<Order>> ByUserId(Guid adrianId)
    {
        var userOrders = _orders.Values
            .Where(o => o.AdrianId == adrianId)
            .ToList()
            .AsReadOnly();

        return Task.FromResult((IReadOnlyCollection<Order>)userOrders);
    }

    public Task<IReadOnlyCollection<Order>> GetAll()
    {
        var allOrders = _orders.Values
            .ToList()
            .AsReadOnly();

        return Task.FromResult((IReadOnlyCollection<Order>)allOrders);
    }

    public Task Save(Order order)
    {
        _orders[order.OrderId] = order;
        SavedOrders.Add(order);
        return Task.CompletedTask;
    }

    public Task Remove(Order order)
    {
        _orders.Remove(order.OrderId);
        return Task.CompletedTask;
    }
}