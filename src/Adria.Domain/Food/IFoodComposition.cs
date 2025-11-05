namespace Adria.Domain.Food;

public interface IFoodComposition
{
    Task<IReadOnlyCollection<FoodComposition>> ByFoodId(string foodId);
    Task<IReadOnlyCollection<FoodComposition>> ByNutrientId(string nutrientId);
    Task Save(FoodComposition foodComposition);
    Task Remove(FoodComposition foodComposition);
}