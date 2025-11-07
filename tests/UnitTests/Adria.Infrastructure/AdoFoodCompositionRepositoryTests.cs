using System.Data.Common;
using System.Net.Http.Headers;
using Adria.Domain.Food;
using Adria.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Logging;
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

    public AdoFoodCompositionRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        _connectionString = "Server=localhost;Port=3306;Database=nutriscan;User=nutri;Password=scan;";
        DbProviderFactories.RegisterFactory(
            "MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);

        _factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");

        _repositoryFood = new AdoFoodRepository(_factory, _connectionString);
        _repositoryNutrient = new AdoNutrientRepository(_factory, _connectionString);

        _repositoryFoodComposition = new AdoFoodCompositionRepository(_factory, _connectionString);
    }

    [Fact]
    public async Task GetAllFoods_ReturnsFoods()
    {
        try
        {
            var foods = await _repositoryFood.GetAll(); 

            Assert.NotNull(foods);
            Assert.NotEmpty(foods);

            foreach (var food in foods)
            {
                _testOutputHelper.WriteLine(
                    $"FoodName: {food?.Name ?? "null"}, Type: {food?.Type ?? "null"}, Edible: {food?.Edible}, Id: {food?.FoodId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in GetAllFoods test:");
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
    
    [Fact]
    public async Task GetAllNutrients()
    {
        try
        {
            var Nutrients = await _repositoryNutrient.GetAllNutrients(); 

            Assert.NotNull(Nutrients);
            Assert.NotEmpty(Nutrients);

            foreach (var nutrient in Nutrients)
            {
                _testOutputHelper.WriteLine(
                    $"Nutrient: {nutrient.Type ?? "null"}, RecommendedAmount: {nutrient.RecommendedAmount}, Id: {nutrient.NutrientId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in GetAllFoods test:");
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
    
    [Fact]
    public async Task SaveFoodComposition_ByNames_Success()
    {
        var foodName = "Banana";
        var foodType = "fruit";
        bool foodEdible = true;
        var nutrientType = "Potassium";
        double amount = 100;

        var foodIds = await _repositoryFood.GetFoodIdByName(foodName);
        Guid foodId = foodIds.FirstOrDefault();
        if (foodId == Guid.Empty)
            foodId = await _repositoryFood.AddFood(foodName, foodType, foodEdible);

        var nutrientIds = await _repositoryNutrient.GetNutrientIdBytype(nutrientType);
        Guid nutrientId = nutrientIds.FirstOrDefault();
        if (nutrientId == Guid.Empty)
            nutrientId = await _repositoryNutrient.AddNutrient(nutrientType, amount);

        var foodComposition = new FoodComposition(foodId, nutrientId, amount);

        await _repositoryFoodComposition.Save(foodComposition);

        Assert.NotEqual(Guid.Empty, foodId);
        Assert.NotEqual(Guid.Empty, nutrientId);

        Assert.True(true);

    }

}
