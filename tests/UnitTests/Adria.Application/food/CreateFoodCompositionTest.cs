using Adria.Application.FoodComposition;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application;

public class CreateFoodCompositionTest
{
    private readonly MockFoodCompositionRepository _repository;
    private readonly CreateFoodComposition _useCase;

    public CreateFoodCompositionTest()
    {
        _repository = new MockFoodCompositionRepository();
        _useCase = new CreateFoodComposition(_repository);
    }

    [Fact]
    public async Task Execute_WithValidInput_SavesFoodComposition()
    {
        var foodId = Guid.NewGuid();
        var nutrientId = "PROTEIN";
        var amount = 25.5;

        var input = new CreateFoodCompositionInput(
            foodId,
            nutrientId,
            amount
        );

        await _useCase.Execute(input);

        Assert.Single(_repository.SavedEntities);

        var saved = _repository.SavedEntities[0];
        Assert.Equal(foodId, saved.FoodId);
        Assert.Equal(nutrientId, saved.NutrientId);
        Assert.Equal(amount, saved.Amount);
    }
}