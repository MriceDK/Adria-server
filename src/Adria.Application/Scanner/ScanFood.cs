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
        Food food = await _getRandomFood.Execute();

        IReadOnlyCollection<NutrientInfo> nutrients =
            await _getFoodNutrients.Execute(new GetFoodNutrientsInput(food.FoodId));

        Guid scanId = Guid.NewGuid();
        DateTime scanDateTime = DateTime.UtcNow;

        Scan scan = new Scan(
            scanId,
            input.AdrianId,
            scanDateTime,
            $"Scanned food: {food.Name}",
            food.FoodId.ToString()
        );

        await _scanRepository.Save(scan);

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