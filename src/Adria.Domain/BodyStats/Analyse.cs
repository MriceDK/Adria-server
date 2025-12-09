namespace Adria.Domain.BodyStats;

public sealed class Analyse
{
    public Analyse(Guid adrianId, DateTime dateTime, List<AnalyseDetail> details, Guid id = default)
    {
        if (adrianId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(adrianId));

        if (details is null || details.Count == 0)
            throw new ArgumentException("Analyse must contain at least one detail.", nameof(details));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        AdrianId = adrianId;
        DateTime = dateTime;
        Details = details;
    }

    public Guid Id { get; private init; }
    public Guid AdrianId { get; private set; }
    public DateTime DateTime { get; private set; }
    public IReadOnlyCollection<AnalyseDetail> Details { get; private set; }
}