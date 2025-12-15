using Adria.Domain.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockSupplementRepository : ISupplementRepository
{
    private readonly List<Supplement> _supplements = new();

    // Used by ById tests and setup
    public Task<Supplement?> ById(Guid supplementId)
    {
        return Task.FromResult(_supplements.FirstOrDefault(s => s.SupplementId == supplementId));
    }

    // Used by CreateSupplement tests
    public Task Save(Supplement supplement)
    {
        // Check for existing supplement and replace it (simulate update)
        var existing = _supplements.FirstOrDefault(s => s.SupplementId == supplement.SupplementId);
        if (existing != null)
        {
            _supplements.Remove(existing);
        }
        _supplements.Add(supplement);
        return Task.CompletedTask;
    }
    
    // Used by DeleteSupplement tests (FIXED IMPLEMENTATION)
    public Task Remove(Supplement supplement)
    {
        var existing = _supplements.FirstOrDefault(s => s.SupplementId == supplement.SupplementId);
        if (existing != null)
        {
            _supplements.Remove(existing);
        }
        return Task.CompletedTask;
    }

    // Implementation for GetAll (Used by SearchAllSupplements)
    public Task<IReadOnlyCollection<Supplement>> GetAll()
    {
        // Return a read-only copy of all supplements
        return Task.FromResult((IReadOnlyCollection<Supplement>)_supplements.AsReadOnly());
    }
    
    // Implementation for ByName
    public Task<IReadOnlyCollection<Supplement>> ByName(string name)
    {
        var found = _supplements
            .Where(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
            
        return Task.FromResult((IReadOnlyCollection<Supplement>)found.AsReadOnly());
    }
    
    // Implementation for ByType
    public Task<IReadOnlyCollection<Supplement>> ByType(string type)
    {
        var found = _supplements
            .Where(s => s.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
            .ToList();
            
        return Task.FromResult((IReadOnlyCollection<Supplement>)found.AsReadOnly());
    }
}