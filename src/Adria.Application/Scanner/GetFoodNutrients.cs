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
        Console.WriteLine($"GETFOODNUTRIENTS: Looking up compositions for FoodId={input.FoodId}");

        IReadOnlyCollection<Domain.Food.FoodComposition> compositions =
            await _foodCompositionRepository.ByFoodId(input.FoodId.ToString());

        Console.WriteLine($"GETFOODNUTRIENTS: Found {compositions.Count} compositions");

        foreach (var c in compositions)
            Console.WriteLine($"GETFOODNUTRIENTS: Composition -> NutrientId={c.NutrientId}, Amount={c.Amount}");

        var results = new List<NutrientInfo>();

        foreach (var comp in compositions)
        {
            Console.WriteLine($"GETFOODNUTRIENTS: Fetching nutrient {comp.NutrientId}");

            Nutrient? nutrient = await _nutrientRepository.ById(comp.NutrientId);

            if (nutrient == null)
            {
                Console.WriteLine($"GETFOODNUTRIENTS: NutrientId {comp.NutrientId} NOT FOUND in nutrient table!");
                continue;
            }

            Console.WriteLine($"GETFOODNUTRIENTS: Nutrient found -> {nutrient.NutrientId}, {nutrient.Type}");

            results.Add(new NutrientInfo(
                nutrient.NutrientId,
                nutrient.Type,
                comp.Amount
            ));
        }

        Console.WriteLine($"GETFOODNUTRIENTS: Returning {results.Count} nutrients");
        return results;
    }

}