using System.Data.Common;
using System.Net.Http.Headers;
using Adria.Domain.Food;
using Adria.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit.Abstractions;

namespace UnitTests.Adria.Infrastructure;

public class AdoFoodCompositionRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AdoFoodRepository _repositoryFood;
    private readonly AdoNutrientRepository _repositoryNutrient;
    private readonly AdoFoodCompositionRepository _repositoryFoodComposition;
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<AdoFoodCompositionRepository> _logger;

    public AdoFoodCompositionRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _connectionString = "Server=localhost;Port=3306;Database=nutriscan;User=nutri;Password=scan;";

        DbProviderFactories.RegisterFactory(
            "MySql.Data.MySqlClient",
            MySql.Data.MySqlClient.MySqlClientFactory.Instance
        );

        _factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");

        _logger = NullLogger<AdoFoodCompositionRepository>.Instance;

        _repositoryFood = new AdoFoodRepository(_factory, _connectionString);
        _repositoryNutrient = new AdoNutrientRepository(_factory, _connectionString);
        _repositoryFoodComposition = new AdoFoodCompositionRepository(
            _factory,
            _connectionString,
            _logger
        );
    }

    [Fact]
    public async Task GetAllFoods_ReturnsFoods()
    {
        var foods = await _repositoryFood.GetAll();
        Assert.NotNull(foods);
        Assert.NotEmpty(foods);

        foreach (var food in foods)
            _testOutputHelper.WriteLine($"FoodName: {food?.Name}, Type: {food?.Type}, Edible: {food?.Edible}, Id: {food?.FoodId}");
    }

    [Fact]
    public async Task GetAllNutrients()
    {
        var nutrients = await _repositoryNutrient.GetAllNutrients();
        Assert.NotNull(nutrients);
        Assert.NotEmpty(nutrients);

        foreach (var nutrient in nutrients)
            _testOutputHelper.WriteLine($"Nutrient: {nutrient.Type}, RecommendedAmount: {nutrient.RecommendedAmount}, Id: {nutrient.NutrientId}");
    }

    [Fact]
    public async Task SaveFoodComposition_ByNames_Success()
    {
        var foodName = "Banana";
        var foodType = "fruit";
        var nutrientType = "Potassium";
        var amount = 100d;

        var foodIds = await _repositoryFood.GetFoodIdByName(foodName);
        var foodId = foodIds.FirstOrDefault();
        if (foodId == Guid.Empty)
            foodId = await _repositoryFood.AddFood(foodName, foodType, true);

        var nutrientIds = await _repositoryNutrient.GetNutrientIdBytype(nutrientType);
        var nutrientId = nutrientIds.FirstOrDefault();
        if (nutrientId == Guid.Empty)
            nutrientId = await _repositoryNutrient.AddNutrient(nutrientType, amount);

        var foodComposition = new FoodComposition(foodId, nutrientId, amount);

        await _repositoryFoodComposition.Save(foodComposition);

        Assert.NotEqual(Guid.Empty, foodId);
        Assert.NotEqual(Guid.Empty, nutrientId);
        Assert.True(true);
    }

}
