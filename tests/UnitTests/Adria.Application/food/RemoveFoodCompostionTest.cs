using Adria.Application.FoodComposition;
using Adria.Domain.Food;
using System;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application;

public sealed class RemoveFoodCompostionTest
{
    [Fact]
    public async Task Execute_WithValidInput_CallsRemoveOnRepository()
    {
        var repository = new MockFoodCompositionRepository();
        var useCase = new RemoveFoodComposition(repository);

        var foodId = Guid.NewGuid();
        var nutrientId = "PROTEIN";
        var amount = 10.0;

        var input = new RemoveFoodCompositionInput(
            foodId,
            nutrientId,
            amount
        );

        var exception = await Record.ExceptionAsync(() => useCase.Execute(input));

        Assert.Null(exception);
    }
}