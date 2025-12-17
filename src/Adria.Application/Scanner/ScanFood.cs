using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.BodyStats;
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
    private readonly IAnalyseRepository _analyseRepository;
    private readonly IBodyStatsQuery _bodyStatsQuery;

    public ScanFood(
        GetRandomFood getRandomFood,
        IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>> getFoodNutrients,
        IScan scanRepository,
        IAnalyseRepository analyseRepository,
        IBodyStatsQuery bodyStatsQuery
    )
    {
        _getRandomFood = getRandomFood;
        _getFoodNutrients = getFoodNutrients;
        _scanRepository = scanRepository;
        _analyseRepository = analyseRepository;
        _bodyStatsQuery = bodyStatsQuery;
    }
    public static List<AnalyseDetail> BuildAnalyseDetails(
        IEnumerable<BodyStatData> bodyStats,
        IEnumerable<NutrientInfo> nutrients)
    {
        var result = new Dictionary<string, double>();

        foreach (var stat in bodyStats)
        {
            if (!result.ContainsKey(stat.BodyStatId))
                result[stat.BodyStatId] = 0;

            result[stat.BodyStatId] += stat.Current;
        }

        foreach (var nutrient in nutrients)
        {
            if (!result.ContainsKey(nutrient.NutrientId))
                result[nutrient.NutrientId] = 0;

            result[nutrient.NutrientId] += nutrient.Amount;
        }

        return result
            .Select(x => new AnalyseDetail(x.Key, x.Value))
            .ToList();
    }

    public async Task<ScannedFoodResult> Execute(ScanFoodInput input)
    {
        Domain.Food.Food food = await _getRandomFood.Execute();

        IReadOnlyCollection<NutrientInfo> nutrients =
            await _getFoodNutrients.Execute(new GetFoodNutrientsInput(food.FoodId));


        Guid scanId = Guid.NewGuid();
        DateTime scanDateTime = DateTime.UtcNow.ToLocalTime();


        Scan scan = new Scan(
            scanId,
            input.AdrianId,
            scanDateTime,
            $"Scanned food: {food.Name}",
            food.FoodId.ToString()
        );

        var lastStat = await _bodyStatsQuery.Fetch(input.AdrianId);

        var detailList = BuildAnalyseDetails(lastStat!, nutrients);
        await _analyseRepository.Save(new Analyse(input.AdrianId, DateTime.Now, detailList));

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