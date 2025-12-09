namespace Adria.Domain.Food;

public interface INutrient
{
    Task<Nutrient?> ById(string nutrientId);
    Task<IReadOnlyCollection<Nutrient>> ByType(string type);
    Task Remove(Nutrient nutrient);
    Task<IReadOnlyCollection<Nutrient>> GetAllNutrients();
    
    Task<IReadOnlyCollection<string>> GetNutrientIdBytype(string type);
}
