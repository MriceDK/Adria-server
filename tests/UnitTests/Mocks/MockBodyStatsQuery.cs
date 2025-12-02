using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace UnitTests.Mocks;

public class MockBodyStatsQuery : IBodyStatsQuery
{
    private readonly Dictionary<Guid, List<BodyStatData>> _dataStore = new();

    public void AddStats(Guid userId, List<BodyStatData> stats)
    {
        _dataStore[userId] = stats;
    }

    public Task<IReadOnlyCollection<BodyStatData>?> Fetch(Guid userId)
    {
        if (_dataStore.TryGetValue(userId, out var stats))
        {
            return Task.FromResult<IReadOnlyCollection<BodyStatData>?>(stats.AsReadOnly());
        }
        
        return Task.FromResult<IReadOnlyCollection<BodyStatData>?>(new List<BodyStatData>().AsReadOnly());
    }
}