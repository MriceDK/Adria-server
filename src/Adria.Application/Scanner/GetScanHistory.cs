using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;
using Adria.Domain.Scanner;

namespace Adria.Application.Scanner;

public sealed record GetScanHistoryInput(Guid AdrianId);

public sealed class GetScanHistory 
    : IUseCase<GetScanHistoryInput, Task<IReadOnlyCollection<ScannedFoodResult>>>
{
    private readonly IScan _scanRepository;
    private readonly IFood _foodRepository;
    private readonly GetFoodNutrients _getFoodNutrients;

    public GetScanHistory(
        IScan scanRepository,
        IFood foodRepository,
        GetFoodNutrients getFoodNutrients
    )
    {
        _scanRepository = scanRepository;
        _foodRepository = foodRepository;
        _getFoodNutrients = getFoodNutrients;
    }

    public async Task<IReadOnlyCollection<ScannedFoodResult>> Execute(GetScanHistoryInput input)
    {
        var scans = await _scanRepository.ByUserId(input.AdrianId);
        var results = new List<ScannedFoodResult>();

        foreach (var scan in scans)
        {
            var food = await _foodRepository.ById(Guid.Parse(scan.FoodId));
            if (food == null)
                continue;

            var nutrients = await _getFoodNutrients.Execute(
                new GetFoodNutrientsInput(food.FoodId)
            );

            results.Add(new ScannedFoodResult(
                scan.ScanId,
                scan.AdrianId,
                food.FoodId,
                food.Name,
                food.Type,
                food.Edible,
                nutrients,
                scan.DateTime
            ));
        }

        return results;
    }
}