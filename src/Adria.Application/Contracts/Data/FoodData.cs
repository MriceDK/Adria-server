namespace Adria.Application.Contracts.Data;

public sealed record FoodData
(
    Guid FoodId,
    string Name,
    string Type,
    bool Edible
);