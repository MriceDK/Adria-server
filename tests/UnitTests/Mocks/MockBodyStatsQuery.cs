using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace UnitTests.Mocks;

public sealed class MockBodyStatsQuery : IBodyStatsQuery
{
    private readonly Dictionary<Guid, IReadOnlyCollection<BodyStatData>> _dataStore = new();

    public void AddStats(Guid userId, IReadOnlyCollection<BodyStatData> stats)
    {
        _dataStore[userId] = stats;
    }

    public Task<IReadOnlyCollection<BodyStatData>?> Fetch(Guid userId)
    {
        if (_dataStore.TryGetValue(userId, out var stats))
        {
            return Task.FromResult<IReadOnlyCollection<BodyStatData>?>(stats);
        }

        return Task.FromResult<IReadOnlyCollection<BodyStatData>?>(Array.Empty<BodyStatData>());
    }
}