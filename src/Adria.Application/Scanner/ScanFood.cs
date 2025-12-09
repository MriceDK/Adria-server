using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;
using Adria.Domain.Scanner;

namespace Adria.Application.Scanner;

public sealed record ScanFoodInput(Guid AdrianId);

public sealed class ScanFood 
    : IUseCase<ScanFoodInput, Task<ScannedFoodResult>>
{
    private readonly GetRandomFood _getRandomFood;
    private readonly IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>> _getFoodNutrients;
    private readonly IScan _scanRepository;

    public ScanFood(
        GetRandomFood getRandomFood,
        IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>> getFoodNutrients,
        IScan scanRepository
    )
    {
        _getRandomFood = getRandomFood;
        _getFoodNutrients = getFoodNutrients;
        _scanRepository = scanRepository;
    }

    public async Task<ScannedFoodResult> Execute(ScanFoodInput input)
    {
        Console.WriteLine("SCANFOOD: Starting scan");

        Domain.Food.Food food = await _getRandomFood.Execute();
        Console.WriteLine($"SCANFOOD: Random food -> Id={food.FoodId}, Name={food.Name}");

        IReadOnlyCollection<NutrientInfo> nutrients =
            await _getFoodNutrients.Execute(new GetFoodNutrientsInput(food.FoodId));

        Console.WriteLine($"SCANFOOD: Nutrients count returned = {nutrients.Count}");

        foreach (var n in nutrients)
            Console.WriteLine($"SCANFOOD: Nutrient -> {n.NutrientId}, Type={n.Type}, Amount={n.Amount}");

        Guid scanId = Guid.NewGuid();
        DateTime scanDateTime = DateTime.UtcNow;

        Console.WriteLine($"SCANFOOD: Saving scan entry...");

        Scan scan = new Scan(
            scanId,
            input.AdrianId,
            scanDateTime,
            $"Scanned food: {food.Name}",
            food.FoodId.ToString()
        );

        await _scanRepository.Save(scan);

        Console.WriteLine("SCANFOOD: Scan saved successfully");

        return new ScannedFoodResult(
            scanId,
            input.AdrianId,
            food.FoodId,
            food.Name,
            food.Type,
            food.Edible,
            nutrients,
            scanDateTime
        );
    }


}