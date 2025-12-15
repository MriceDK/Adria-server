using Adria.Application.Contracts.Data;
using Adria.Application.Order;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;
using Xunit;
using System;
using System.Threading.Tasks;

namespace UnitTests.Adria.Application.Order;

public sealed class SearchOrderByIdTests
{
    [Fact]
    public async Task Execute_WithExistingOrder_ReturnsOrderData()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderData = new OrderData(
            orderId, 
            Guid.NewGuid(), 
            DateTime.Now, 
            45.99
        );
        
        var mockQuery = new MockOrderByIdQuery();
        mockQuery.AddOrder(orderData);
        
        var mockLogger = new MockLogger<SearchOrderById>();
        var useCase = new SearchOrderById(mockQuery, mockLogger);
        var input = new SearchOrderByIdInput(orderId);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.OrderId);
        Assert.Equal(45.99, result.TotalPrice);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching order with ID {orderId}", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNonExistentOrder_ThrowsElementNotFoundException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var mockQuery = new MockOrderByIdQuery();
        var mockLogger = new MockLogger<SearchOrderById>();
        var useCase = new SearchOrderById(mockQuery, mockLogger);
        var input = new SearchOrderByIdInput(orderId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => useCase.Execute(input)
        );
        
        Assert.Contains($"Order with ID {orderId} not found", exception.Message);
    }

    [Fact]
    public async Task Execute_WithEmptyGuid_ThrowsElementNotFoundException()
    {
        // Arrange
        var orderId = Guid.Empty;
        var mockQuery = new MockOrderByIdQuery();
        var mockLogger = new MockLogger<SearchOrderById>();
        var useCase = new SearchOrderById(mockQuery, mockLogger);
        var input = new SearchOrderByIdInput(orderId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => useCase.Execute(input)
        );
        
        Assert.Contains($"Order with ID {orderId} not found", exception.Message);
    }
}