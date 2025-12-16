using Adria.Application.FoodComposition;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application;

public sealed class SearchFoodCompositionByNameTest
{
    [Fact]
    public async Task Execute_WithExistingFoodId_ReturnsMatchingFoodCompositions()
    {
        var foodId = Guid.NewGuid();

        var compositions = new[]
        {
            new FoodComposition(foodId, "PROTEIN", 25),
            new FoodComposition(foodId, "CARBS", 40),
            new FoodComposition(Guid.NewGuid(), "FAT", 10)
        };

        var repository = new MockFoodCompositionRepository(compositions);
        var useCase = new SearchFoodCompositionsByFoodName(repository);

        var input = new SearchFoodCompositionsByFoodIdInput(foodId.ToString());

        var result = await useCase.Execute(input);

        Assert.Equal(2, result.Count);

        var protein = result.First(r => r.NutrientId == "PROTEIN");
        Assert.Equal(25, protein.Amount);

        var carbs = result.First(r => r.NutrientId == "CARBS");
        Assert.Equal(40, carbs.Amount);
    }
}