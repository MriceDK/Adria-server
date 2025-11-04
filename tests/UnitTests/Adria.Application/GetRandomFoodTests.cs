using Adria.Application.Scanner;
using Adria.Domain.Food;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class GetRandomFoodTests
{
    [Fact]
    public async Task GetRandomFood_ReturnsAValidFood()
    {
        // Arrange
        Guid food1Id = Guid.NewGuid();
        Guid food2Id = Guid.NewGuid();
        Food chicken = new Food(food1Id, "Chicken Breast", "Poultry", true);
        Food pasta = new Food(food2Id, "Pasta", "Grains", true);
        IReadOnlyCollection<Food> foods = new List<Food> { chicken, pasta };
        IFood mockFoodRepo = new MockFoodRepository(foods);

        GetRandomFood getRandomFood = new GetRandomFood(mockFoodRepo);

        // Act
        Food result = await getRandomFood.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Contains(result, foods);
        Assert.False(string.IsNullOrWhiteSpace(result.Name));
        Assert.False(string.IsNullOrWhiteSpace(result.Type));
    }


}