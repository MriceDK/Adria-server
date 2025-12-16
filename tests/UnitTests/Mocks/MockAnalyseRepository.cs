using Adria.Domain.BodyStats;

namespace UnitTests.Mocks;

public class MockAnalyseRepository : IAnalyseRepository
{
    private readonly List<Analyse> _analyses = new();
    public List<Analyse> SavedAnalyses { get; } = new();

    public Task Save(Analyse analyse)
    {
        _analyses.Add(analyse);
        SavedAnalyses.Add(analyse);

        return Task.CompletedTask;
    }

    public Task<Analyse?> ById(Guid id)
    {
        var analyse = _analyses.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(analyse);
    }
}