using System.Collections.ObjectModel;
using Adria.Application.Contracts.Data;
using Adria.Application.FoodComposition;
using Adria.Domain.Food;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.FoodComposition;

public class GetAllFoodsTests
{
    [Fact]
    public async Task Execute_WithExistingFoods_ReturnsAllFoodData()
    {
        // Arrange
        var food1 = new Food(
            foodId: Guid.NewGuid(),
            name: "Apple",
            type: "Fruit",
            edible: true
        );

        var food2 = new Food(
            foodId: Guid.NewGuid(),
            name: "Chicken",
            type: "Meat",
            edible: true
        );

        var mockRepository =
            new MockFoodRepository(new ReadOnlyCollection<Food>(new List<Food> { food1, food2 }));

        var useCase = new GetAllFoods(mockRepository);

        // Act
        var result = await useCase.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var firstFood = result.First(f => f.FoodId == food1.FoodId);
        Assert.Equal("Apple", firstFood.Name);
        Assert.Equal("Fruit", firstFood.Type);
        Assert.True(firstFood.Edible);

        var secondFood = result.First(f => f.FoodId == food2.FoodId);
        Assert.Equal("Chicken", secondFood.Name);
        Assert.Equal("Meat", secondFood.Type);
        Assert.True(secondFood.Edible);
    }

    [Fact]
    public async Task Execute_WithNoFoods_ReturnsEmptyCollection()
    {
        // Arrange
        var mockRepository = new MockFoodRepository(new ReadOnlyCollection<Food>(new List<Food> {}));

        var useCase = new GetAllFoods(mockRepository);

        // Act
        var result = await useCase.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}