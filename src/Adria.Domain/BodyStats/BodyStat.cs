namespace Adria.Domain.BodyStats;

public sealed class BodyStat
{
    public BodyStat(string label, string? unit, double? goal, string? id = null)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label cannot be empty.", nameof(label));

        Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
        Label = label;
        Unit = unit;
        Goal = goal;
    }

    public string Id { get; private init; }
    public string Label { get; private set; }
    public string? Unit { get; private set; }
    public double? Goal { get; private set; }

    public void UpdateGoal(double? newGoal)
    {
        if (newGoal is < 0)
        {
            throw new ArgumentException("Goal cannot be negative.", nameof(newGoal));
        }

        Goal = newGoal;
    }
}