using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace Adria.Application.Scanner;

public sealed record GetFoodNutrientsInput(Guid FoodId);

public sealed class GetFoodNutrients 
    : IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>>
{
    private readonly IFoodComposition _foodCompositionRepository;
    private readonly INutrient _nutrientRepository;

    public GetFoodNutrients(IFoodComposition foodCompositionRepository, INutrient nutrientRepository)
    {
        _foodCompositionRepository = foodCompositionRepository;
        _nutrientRepository = nutrientRepository;
    }

    public async Task<IReadOnlyCollection<NutrientInfo>> Execute(GetFoodNutrientsInput input)
    {

        IReadOnlyCollection<Domain.Food.FoodComposition> compositions =
            await _foodCompositionRepository.ByFoodId(input.FoodId.ToString());


        var results = new List<NutrientInfo>();

        foreach (var comp in compositions)
        {

            Nutrient? nutrient = await _nutrientRepository.ById(comp.NutrientId);

    


            results.Add(new NutrientInfo(
                nutrient.NutrientId,
                nutrient.Type,
                comp.Amount
            ));
        }

        return results;
    }

}