using Adria.Domain.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockSupplementRepository : ISupplementRepository
{
    private readonly List<Supplement> _supplements = new();

    public Task<Supplement?> ById(Guid supplementId)
    {
        return Task.FromResult(_supplements.FirstOrDefault(s => s.SupplementId == supplementId));
    }

    // Implementation for Save (the core method used by the use case)
    public Task Save(Supplement supplement)
    {
        // Check for existing supplement and update, otherwise add
        var existing = _supplements.FirstOrDefault(s => s.SupplementId == supplement.SupplementId);
        if (existing != null)
        {
            _supplements.Remove(existing);
        }
        _supplements.Add(supplement);
        return Task.CompletedTask;
    }

    // Required by the interface but not used by CreateSupplement use case
    public Task<IReadOnlyCollection<Supplement>> ByName(string name) => throw new NotImplementedException();
    public Task<IReadOnlyCollection<Supplement>> ByType(string type) => throw new NotImplementedException();
    public Task<IReadOnlyCollection<Supplement>> GetAll() => throw new NotImplementedException();
    public Task Remove(Supplement supplement) => throw new NotImplementedException();
}