using Adria.Application.Food;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;
using System;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application;

public sealed class CreateNewFoodTest
{
    [Fact]
    public async Task Execute_WithValidInput_SavesFoodAndReturnsId()
    {
        var repository = new MockFoodRepository();
        var useCase = new CreateNewFood(repository);

        var input = new FoodData(
            Guid.NewGuid(),
            "Apple",
            "Fruit",
            true
        );

        var resultId = await useCase.Execute(input);

        Assert.Single(repository.SavedEntities);

        var saved = repository.SavedEntities[0];
        Assert.Equal(resultId, saved.FoodId);
        Assert.Equal(input.Name, saved.Name);
        Assert.Equal(input.Type, saved.Type);
        Assert.Equal(input.Edible, saved.Edible);
    }
}