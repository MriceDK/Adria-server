namespace Adria.Application.Contracts.Data;

public sealed record NutrientInfo(
    Guid NutrientId,
    string Type,
    double Amount,
    double RecommendedAmount
);