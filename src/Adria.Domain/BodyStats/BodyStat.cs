namespace Adria.Domain.BodyStatus;

public sealed class BodyStat
{
    public BodyStat(string label, string? unit, double? goal, Guid id = default)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label cannot be empty.", nameof(label));

        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Label = label;
        Unit = unit;
        Goal = goal;
    }

    public Guid Id { get; private init; }
    public string Label { get; private set; }
    public string? Unit { get; private set; }
    public double? Goal { get; private set; }
}