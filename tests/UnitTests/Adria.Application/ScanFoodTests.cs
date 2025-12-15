using Adria.Application.Contracts.Data;
using Adria.Application.Scanner;
using Adria.Domain.Food;
using UnitTests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Adria.Application;

public class ScanFoodTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private static readonly string[] COLLECTION = ["Chicken Breast", "Broccoli"];

    public ScanFoodTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ScanFood_UserRequestsScan_FoodAndNutrientsAreLoaded()
    {
        Guid adrianId = Guid.NewGuid();
        Guid foodId = Guid.NewGuid();
        string foodName = "Chicken Breast";
        string foodType = "Poultry";
        bool foodEdible = true;

        var food = new Food(foodId, foodName, foodType, foodEdible);
        var nutrient = new Nutrient("bd1-prot-0001", "Protein", "g");
        var composition = new FoodComposition(foodId, "bd1-prot-0001", 31.0);

        var mockFoodRepo = new MockFoodRepository(new List<Food> { food });
        var mockNutrientRepo = new MockNutrientRepository(new List<Nutrient> { nutrient });
        var mockFoodCompRepo = new MockFoodCompositionRepository(new List<FoodComposition> { composition });
        var mockScanRepo = new MockScanRepository();

        var getRandomFood = new GetRandomFood(mockFoodRepo);
        var getFoodNutrients = new GetFoodNutrients(mockFoodCompRepo, mockNutrientRepo);
        var scanFoodUseCase = new ScanFood(getRandomFood, getFoodNutrients, mockScanRepo);

        ScannedFoodResult result = await scanFoodUseCase.Execute(new ScanFoodInput(adrianId));

        _testOutputHelper.WriteLine($"Foodname: {result.FoodName}");
        foreach (var nut in result.Nutrients)
        {
            _testOutputHelper.WriteLine(
                $"Nutrient: {nut.Type}, Amount: {nut.Amount}, ScanTime: {result.ScanDateTime}");
        }

        Assert.Equal(foodName, result.FoodName);
        Assert.Single(result.Nutrients);
    }

    [Fact]
    public async Task ScanFood_WithMultipleFoodsAndNutrients_ReturnsFoodAndAllNutrients()
    {
        Guid adrianId = Guid.NewGuid();

        Guid food1Id = Guid.NewGuid();
        var food1 = new Food(food1Id, "Chicken Breast", "Poultry", true);

        Guid food2Id = Guid.NewGuid();
        var food2 = new Food(food2Id, "Broccoli", "Vegetable", true);

        var protein = new Nutrient("bd1-prot-0001", "Protein", "g");
        var Carbohydrates = new Nutrient("bd1-carb-0002", "Carbohydrates", "g");
        var Calories = new Nutrient("bd1-cali-0005", "Calories", "g");

        var compositions = new List<FoodComposition>
        {
            new(food1Id, "bd1-cali-0005", 31.0),
            new(food1Id, "bd1-prot-0001", 0.5),

            new(food2Id, "bd1-cali-0005", 2.8),
            new(food2Id, "bd1-prot-0001", 2.6),
            new(food2Id, "bd1-carb-0002", 89.0)
        };

        var mockFoodRepo = new MockFoodRepository(new List<Food> { food1, food2 });
        var mockNutrientRepo = new MockNutrientRepository(new List<Nutrient> { protein, Carbohydrates, Calories });
        var mockFoodCompRepo = new MockFoodCompositionRepository(compositions);
        var mockScanRepo = new MockScanRepository();

        var getRandomFood = new GetRandomFood(mockFoodRepo);
        var getFoodNutrients = new GetFoodNutrients(mockFoodCompRepo, mockNutrientRepo);
        var scanFoodUseCase = new ScanFood(getRandomFood, getFoodNutrients, mockScanRepo);

        ScannedFoodResult result = await scanFoodUseCase.Execute(new ScanFoodInput(adrianId));

        _testOutputHelper.WriteLine($"Foodname: {result.FoodName}");
        foreach (var nutrient in result.Nutrients)
        {
            _testOutputHelper.WriteLine(
                $"Nutrient: {nutrient.Type}, Amount: {nutrient.Amount}");
        }
        _testOutputHelper.WriteLine(
            $"{result.FoodName}{result.Nutrients.Count}{result.ScanDateTime}{result.ScanId}");

        Assert.Contains(result.FoodName, COLLECTION);
        Assert.True(result.Nutrients.Count > 0);
    }
}
