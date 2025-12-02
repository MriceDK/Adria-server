namespace Adria.Application.Contracts.Data;

public record FoodCompositionData(
    Guid FoodId,
    Guid NutrientId,
    double Amount
    );