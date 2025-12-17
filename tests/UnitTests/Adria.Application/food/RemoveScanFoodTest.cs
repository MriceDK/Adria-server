using Adria.Application.Scanner;
using Adria.Application.Contracts.Data;
using Adria.Domain.Scanner;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public sealed class RemoveScanFoodTest
{
    [Fact]
    public async Task Execute_WithValidScan_RemovesScanAndCreatesAnalyse()
    {
        var scanRepository = new MockScanRepository();
        var analyseRepository = new MockAnalyseRepository();
        var bodyStatsQuery = new MockBodyStatsQuery();
        var getFoodNutrients = new MockGetFoodNutrientsUseCase();

        var useCase = new RemoveScanFood(
            scanRepository,
            analyseRepository,
            bodyStatsQuery,
            getFoodNutrients
        );

        var scanId = Guid.NewGuid();
        var foodId = Guid.NewGuid();
        var adrianId = Guid.NewGuid();

        await scanRepository.Save(
            new Scan(
                scanId,
                adrianId,
                DateTime.UtcNow.ToLocalTime(),
                "OK",
                foodId.ToString()
            )
        );

        bodyStatsQuery.AddStats(
            adrianId,
            new[]
            {
                new BodyStatData(
                    "CALORIES",
                    "Calories",
                    2000,
                    null,
                    null
                )            
            }
        );

        getFoodNutrients.SetReturnValue(
            new[]
            {
                new NutrientInfo(
                    "CALORIES",
                    "Calories",
                    500,
                    "kcal"
                )
            }
        );


        

        var input = new RemoveScanFoodInput(scanId);

        var exception = await Record.ExceptionAsync(() => useCase.Execute(input));

        Assert.Null(exception);
        Assert.Single(analyseRepository.SavedAnalyses);
        Assert.Empty(await scanRepository.ByUserId(adrianId));
    }
}