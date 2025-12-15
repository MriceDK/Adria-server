using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.BodyStats;
using Adria.Domain.Scanner;
using IBodyStatRepository = Adria.Application.Contracts.IBodyStatRepository;

namespace Adria.Application.Scanner;

public sealed record RemoveScanFoodInput(Guid ScanId);

public sealed class RemoveScanFood
    : IUseCase<RemoveScanFoodInput, Task>
{
    private readonly IScan _scanRepository;
    private readonly IAnalyseRepository _analyseRepository;
    private IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>> getFoodNutrients;
    private readonly IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>> _getFoodNutrients;
    private readonly IBodyStatsQuery _bodyStatsQuery;

    public RemoveScanFood(IScan scanRepository, IAnalyseRepository analyseRepository,
        IBodyStatsQuery bodyStatsQuery,IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>> getFoodNutrients)
    {
        _scanRepository = scanRepository;
        _analyseRepository = analyseRepository;
        _bodyStatsQuery = bodyStatsQuery;
        _getFoodNutrients = getFoodNutrients;

    }

    public async Task Execute(RemoveScanFoodInput input)
    {
        var scan = (await _scanRepository.ById(input.ScanId))!;
        
        IReadOnlyCollection<NutrientInfo> nutrients = await _getFoodNutrients.Execute(new GetFoodNutrientsInput(new Guid(scan.FoodId)));
        var lastStat = await _bodyStatsQuery.Fetch(scan.AdrianId);
        
        var analyseDetails = BuildAnalyseDetails(lastStat!, nutrients);
        
        await _analyseRepository.Save(new Analyse(scan.AdrianId, DateTime.Now, analyseDetails));
        await _scanRepository.Remove(input.ScanId);
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

            result[nutrient.NutrientId] -= nutrient.Amount;
        }

        return result
            .Select(x => new AnalyseDetail(x.Key, x.Value))
            .ToList();
    }
}