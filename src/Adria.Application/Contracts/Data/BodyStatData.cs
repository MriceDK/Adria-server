namespace Adria.Application.Contracts.Data;

public sealed record BodyStatData(
    string Label,
    string Current,
    double? Goal,
    string? Unit
);