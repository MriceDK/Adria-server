namespace Adria.Domain.Food;

public interface INutrient
{
    Task<Nutrient?> ById(string nutrientId);
    Task<IReadOnlyCollection<Nutrient>> ByType(string type);
    Task Save(Nutrient nutrient);
    Task Remove(Nutrient nutrient);
}
