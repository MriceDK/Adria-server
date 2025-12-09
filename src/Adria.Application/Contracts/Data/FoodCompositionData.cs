namespace Adria.Application.Contracts.Data;

public record FoodCompositionData(
    Guid FoodId,
    string NutrientId,
    double Amount
    );