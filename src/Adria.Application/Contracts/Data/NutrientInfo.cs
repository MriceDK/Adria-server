namespace Adria.Application.Contracts.Data;

public sealed record NutrientInfo(
    string NutrientId,
    string Type,
    double Amount,
    string Unit
);