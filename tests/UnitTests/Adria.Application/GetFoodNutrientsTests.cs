using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace UnitTests.Adria.Application;

public class GetFoodNutrientsTests
{
    [Fact]
    public async Task Execute_WithValidFoodId_ReturnsCorrectNutrientInfos()
    {
        // Arrange
        Guid foodId = Guid.NewGuid();
        Guid nutrientId = Guid.NewGuid();
        double expectedAmount = 31.0;
        double expectedRecommended = 50.0;

        var comp = new FoodComposition(foodId, nutrientId, expectedAmount);
        var nutrient = new Nutrient(nutrientId, "Protein", expectedRecommended);

        var comps = new List<FoodComposition> { comp };
       // var mockFoodCompositionRepo = new Mock<IFoodComposition>();
       // mockFoodCompositionRepo.Setup(repo => repo.ByFoodId(foodId.ToString())).ReturnsAsync(comps);

       // var mockNutrientRepo = new Mock<INutrient>();
//mockNutrientRepo.Setup(repo => repo.ById(nutrientId.ToString())).ReturnsAsync(nutrient);

        //var useCase = new GetFoodNutrients(mockFoodCompositionRepo.Object, mockNutrientRepo.Object);

        // Act
        //IReadOnlyCollection<NutrientInfo> result = await useCase.Execute(foodId);

        // Assert
       // Assert.Single(result);
       // var info = result.First();
        //Assert.Equal(nutrientId, info.NutrientId);
       // Assert.Equal("Protein", info.Type);
       // Assert.Equal(expectedAmount, info.Amount);
        //Assert.Equal(expectedRecommended, info.RecommendedAmount);
    }

}