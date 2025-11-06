using System.Data.Common;
using Adria.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace UnitTests.Adria.Infrastructure;

public class AdoScanRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AdoFoodRepository _repository;

    public AdoScanRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var testConnectionString = "Server=localhost;Port=3306;Database=nutriscan;User=nutri;Password=scan;";
        DbProviderFactories.RegisterFactory(
            "MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
        var factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");

        _repository = new AdoFoodRepository(factory, testConnectionString);
    }

    [Fact]
    public async Task GetAllFoods_ReturnsFoods()
    {
        try
        {
            var foods = await _repository.GetAll(); 

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
}
