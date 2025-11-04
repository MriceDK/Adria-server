namespace Adria.Application.Contracts.Data;

public sealed record ScannedFoodResult(

    Guid ScanId,
    Guid AdrianId,
    Guid FoodId,
    string FoodName,
    string FoodType,
    bool FoodEdible,
    IReadOnlyCollection<NutrientInfo> Nutrients,
    DateTime ScanDateTime
);