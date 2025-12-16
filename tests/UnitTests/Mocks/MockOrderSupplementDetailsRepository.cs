using Adria.Domain.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockOrderSupplementDetailsRepository : IOrderSupplementDetailsRepository
{
    private readonly List<OrderSupplementDetails> _entities = new();

    public IReadOnlyList<OrderSupplementDetails> Entities => _entities;

    // Backward compatibility for previous tests
    public List<OrderSupplementDetails> SavedEntities => _entities;

    public void Seed(OrderSupplementDetails entity)
    {
        _entities.Add(entity);
    }

    public Task Save(OrderSupplementDetails entity)
    {
        _entities.Add(entity);
        return Task.CompletedTask;
    }

    public Task Remove(OrderSupplementDetails orderSupplementDetails)
    {
        _entities.Remove(orderSupplementDetails);
        return Task.CompletedTask;
    }

    public Task<OrderSupplementDetails?> ByOrderAndSupplementId(Guid orderId, Guid supplementId)
    {
        return Task.FromResult<OrderSupplementDetails?>(
            _entities.FirstOrDefault(e =>
                e.OrderId == orderId &&
                e.SupplementId == supplementId)
        );
    }

    public Task<IReadOnlyCollection<OrderSupplementDetails>> ByOrderId(Guid orderId)
    {
        return Task.FromResult<IReadOnlyCollection<OrderSupplementDetails>>(
            _entities.Where(e => e.OrderId == orderId).ToList()
        );
    }

    public Task<IReadOnlyCollection<OrderSupplementDetails>> BySupplementId(Guid supplementId)
    {
        return Task.FromResult<IReadOnlyCollection<OrderSupplementDetails>>(
            _entities.Where(e => e.SupplementId == supplementId).ToList()
        );
    }
}