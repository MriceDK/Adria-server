namespace Adria.Domain.Food;

public interface IFoodComposition
{
    Task<IReadOnlyCollection<FoodComposition>> ByFoodId(string foodName);
    Task<IReadOnlyCollection<FoodComposition>> ByNutrientId(string type);
    Task Save(FoodComposition foodComposition);
    Task Remove(FoodComposition foodComposition);
}