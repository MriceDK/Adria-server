using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace Adria.Application.Scanner;

public class GetFoodNutrients
{
    private readonly IFoodComposition _foodCompositionRepository;
    private readonly INutrient _nutrientRepository;

    public GetFoodNutrients(IFoodComposition foodCompositionRepository, INutrient nutrientRepository)
    {
        _foodCompositionRepository = foodCompositionRepository;
        _nutrientRepository = nutrientRepository;
    }

    public async Task<IReadOnlyCollection<NutrientInfo>> Execute(Guid foodId)
    {
        IReadOnlyCollection<FoodComposition>
            compositions = await _foodCompositionRepository.ByFoodId(foodId.ToString());
        List<NutrientInfo> results = new List<NutrientInfo>();
        foreach (FoodComposition comp in compositions)
        {
            Nutrient? nutrient = await _nutrientRepository.ById(comp.NutrientId.ToString());
            if (nutrient != null)
            {
                NutrientInfo info = new NutrientInfo(
                    nutrient.NutrientId,
                    nutrient.Type,
                    comp.Amount,
                    nutrient.RecommendedAmount
                );
                results.Add(info);
            }
        }

        return results.AsReadOnly();
    }
}