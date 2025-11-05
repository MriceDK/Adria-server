using Adria.Domain.Order;

namespace UnitTests.Adria.Domain;

public class SupplementTests
{
    [Fact]
    public void CreateSupplementTest()
    {
        // Arrange
        var supplementId = Guid.NewGuid();
        var name = "Vitamin C";
        var type = "Vitamin";
        var price = 19.99;

        // Act
        var supplement = new Supplement(supplementId, name, type, price);

        // Assert
        Assert.Equal(supplementId, supplement.SupplementId);
        Assert.Equal(name, supplement.Name);
        Assert.Equal(type, supplement.Type);
        Assert.Equal(price, supplement.Price);
    }

    [Fact]
    public void SetPrice_ShouldThrow_WhenNegative()
    {
        // Arrange
        var supplement = new Supplement(Guid.NewGuid(), "Vitamin C", "Vitamin", 10);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => supplement.SetPrice(-5));
        Assert.Equal("New price cannot be negative (Parameter 'price')", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetName_ShouldThrow_WhenInvalid(string invalidName)
    {
        // Arrange
        var supplement = new Supplement(Guid.NewGuid(), "Vitamin C", "Vitamin", 10);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => supplement.SetName(invalidName));
        Assert.Equal("Name cannot be null or empty. (Parameter 'name')", ex.Message);
    }

    [Fact]
    public void SetNameAndType_ShouldUpdateValues()
    {
        // Arrange
        var supplement = new Supplement(Guid.NewGuid(), "Vitamin C", "Vitamin", 10);
        var newName = "Omega 3";
        var newType = "Fatty Acid";

        // Act
        supplement.SetName(newName);
        supplement.SetType(newType);

        // Assert
        Assert.Equal(newName, supplement.Name);
        Assert.Equal(newType, supplement.Type);
    }
}