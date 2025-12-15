namespace Adria.Application.Contracts.Data;

public sealed record BodyStatData(
    string BodyStatId,
    string Label,
    double Current,
    double? Goal,
    string? Unit
);