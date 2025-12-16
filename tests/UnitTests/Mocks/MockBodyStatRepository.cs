using Adria.Application.Contracts;
using Adria.Domain.BodyStats;
using System.Collections.Generic;
using System.Threading.Tasks;
using IBodyStatRepository = Adria.Domain.BodyStats.IBodyStatRepository;

namespace UnitTests.Mocks;

public sealed class MockBodyStatRepository : IBodyStatRepository, global::Adria.Application.Contracts.IBodyStatRepository
{
    private readonly Dictionary<string, BodyStat> _store = new();

    public List<BodyStat> UpdatedEntities { get; } = new();

    public void Seed(BodyStat bodyStat)
    {
        _store[bodyStat.Id] = bodyStat;
    }

    public Task<BodyStat?> ById(string bodyStatId)
    {
        _store.TryGetValue(bodyStatId, out var bodyStat);
        return Task.FromResult(bodyStat);
    }

    public Task Save(BodyStat bodyStat)
    {
        throw new NotImplementedException();
    }

    public Task Update(BodyStat bodyStat)
    {
        UpdatedEntities.Add(bodyStat);
        return Task.CompletedTask;
    }
}