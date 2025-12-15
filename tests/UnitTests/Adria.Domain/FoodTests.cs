using Adria.Domain.Food;

namespace UnitTests.Adria.Domain;

public class FoodTests
{
    [Fact]
    public void CreateFoodTest()
    {
        // Arrange
        var foodId = Guid.NewGuid();
        var name = "Chicken Breast";
        var type = "Poultry";
        var edible = true;

        // Act
        var food = new Food(foodId ,name,type,edible);

        // Assert
        Assert.Equal(name, food.Name);
        Assert.Equal(type, food.Type);
        Assert.True(food.Edible);
        Assert.False(string.IsNullOrWhiteSpace(food.FoodId.ToString()));
    }
    
    [Fact]
    public void Constructor_WithTypeAndAmount_CreatesNutrient()
    {
        // Arrange
        var nutrientId = "bd1-prot-0001";
        var type = "Protein";
        var unit = "g";

        // Act
        var nutrient = new Nutrient(nutrientId, type,unit);

        // Assert
        Assert.Equal(type, nutrient.Type);
        Assert.Equal(unit, nutrient.Unit);
        Assert.False(string.IsNullOrWhiteSpace(nutrient.NutrientId.ToString()));
    }
    
    [Fact]
    public void Constructor_WithValidProperties_CreatesFoodComposition()
    {
        // Arrange
        var foodId = Guid.NewGuid();
        var nutrientId = "bd1-prot-0001";

        var food = new Food(foodId,"Chicken Breast", "Poultry", true);
        var nutrient = new Nutrient(nutrientId,"Protein", "g");
        
        double amount = 20;

        // Act
        var comp = new FoodComposition(food.FoodId, nutrient.NutrientId, amount);
        
        // Assert
        Assert.Equal(food.FoodId, comp.FoodId);
        Assert.Equal(nutrient.NutrientId, comp.NutrientId);
        Assert.Equal(amount, comp.Amount);
    }
    [Fact]
    public void Constructor_WithEmptyFoodId_ThrowsArgumentException()
    {
        // Arrange
        var emptyFoodId = Guid.Empty;
        var nutrientId = "bd1-prot-0001";
        double amount = 10;

        // Act
        var ex = Assert.Throws<ArgumentException>(
            () => new FoodComposition(emptyFoodId, nutrientId, amount));

        // Assert
        Assert.Equal("ID cannot be null or empty. (Parameter 'foodId')", ex.Message);
        Assert.Equal("foodId", ex.ParamName);
    }
    
    [Fact]
    public void ChangeAmount_WithAmountCausingValidationFailure_ThrowsArgumentException()
    {
        // Arrange
        var comp = new FoodComposition(Guid.NewGuid(), "bd1-prot-0001", 10);

        Assert.Throws<ArgumentException>(() => comp.ChangeAmount(-20));
    }
}