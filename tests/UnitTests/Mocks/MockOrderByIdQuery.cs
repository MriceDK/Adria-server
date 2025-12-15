using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

// Assuming OrderData exists in Adria.Application.Contracts.Data
// public sealed record OrderData(Guid OrderId, Guid AdrianId, DateTime Date, double TotalPrice);

// Assuming IOrderByIdQuery exists in Adria.Application.Contracts
// public interface IOrderByIdQuery { Task<OrderData?> Fetch(Guid orderId); }

public sealed class MockOrderByIdQuery : IOrderByIdQuery
{
    private readonly Dictionary<Guid, OrderData> _dataStore = new();

    public void AddOrder(OrderData data)
    {
        _dataStore[data.OrderId] = data;
    }
    
    public Task<OrderData?> Fetch(Guid orderId)
    {
        _dataStore.TryGetValue(orderId, out var data);
        return Task.FromResult(data);
    }
}