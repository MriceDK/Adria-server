using Adria.Domain.Order;

namespace UnitTests.Adria.Domain;

public class OrderSupplementDetailsTests
{
    [Fact]
    public void CreateOrderSupplementDetailsTest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var supplementId = Guid.NewGuid();
        var amount = 5;

        // Act
        var details = new OrderSupplementDetails(orderId, supplementId, amount);

        // Assert
        Assert.Equal(orderId, details.OrderId);
        Assert.Equal(supplementId, details.SupplementId);
        Assert.Equal(amount, details.Amount);
    }

    [Fact]
    public void SetAmount_ShouldThrow_WhenNegative()
    {
        // Arrange
        var details = new OrderSupplementDetails(Guid.NewGuid(), Guid.NewGuid(), 2);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => details.SetAmount(-1));
        Assert.Equal("New price cannot be negative (Parameter 'amount')", ex.Message);
    }

    [Fact]
    public void SetAmount_ShouldUpdateAmount()
    {
        // Arrange
        var details = new OrderSupplementDetails(Guid.NewGuid(), Guid.NewGuid(), 2);
        var newAmount = 10;

        // Act
        details.SetAmount(newAmount);

        // Assert
        Assert.Equal(newAmount, details.Amount);
    }
}