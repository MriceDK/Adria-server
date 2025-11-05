using Adria.Domain.Order;

namespace UnitTests.Adria.Domain;

public class OrderTests
{
    [Fact]
    public void CreateOrderTest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var adrianId = Guid.NewGuid();
        var date = DateTime.UtcNow;
        var totalPrice = 99.99;

        // Act
        var order = new Order(orderId, adrianId, date, totalPrice);

        // Assert
        Assert.Equal(orderId, order.OrderId);
        Assert.Equal(adrianId, order.AdrianId);
        Assert.Equal(date, order.Date);
        Assert.Equal(totalPrice, order.TotalPrice);
    }

    [Fact]
    public void SetTotalPrice_ShouldThrow_WhenNegative()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 10);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => order.SetTotalPrice(-5));
        Assert.Equal("New price cannot be negative (Parameter 'totalPrice')", ex.Message);
    }

    [Fact]
    public void SetDate_ShouldUpdateDate()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 50);
        var newDate = DateTime.UtcNow.AddDays(1);

        // Act
        order.SetDate(newDate);

        // Assert
        Assert.Equal(newDate, order.Date);
    }
}