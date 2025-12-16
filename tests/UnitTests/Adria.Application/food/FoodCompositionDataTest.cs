using Adria.Application.Contracts.Data;
using System;
using Xunit;

namespace UnitTests.Adria.Application;

public sealed class FoodCompositionDataTest
{
    [Fact]
    public void Constructor_WithValidValues_SetsAllPropertiesCorrectly()
    {
        var foodId = Guid.NewGuid();
        var nutrientId = "PROTEIN";
        var amount = 12.5;

        var data = new FoodCompositionData(
            foodId,
            nutrientId,
            amount
        );

        Assert.Equal(foodId, data.FoodId);
        Assert.Equal(nutrientId, data.NutrientId);
        Assert.Equal(amount, data.Amount);
    }
}