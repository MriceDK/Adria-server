using Adria.Application.Order;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;
using Xunit;
using System;
using System.Threading.Tasks;

namespace UnitTests.Adria.Application.Order;

public sealed class DeleteOrderTests
{
    private readonly MockOrderRepository _mockOrderRepository;
    private readonly MockLogger<DeleteOrder> _mockLogger;
    private readonly DeleteOrder _useCase;

    private readonly Guid _existingOrderId = Guid.NewGuid();
    private readonly Guid _nonExistingOrderId = Guid.NewGuid();
    private readonly global::Adria.Domain.Order.Order _existingOrder;

    public DeleteOrderTests()
    {
        // Arrange
        _mockOrderRepository = new MockOrderRepository();
        _mockLogger = new MockLogger<DeleteOrder>();

        // Create and save an order that will be deleted in a test case
        _existingOrder = new global::Adria.Domain.Order.Order(_existingOrderId, Guid.NewGuid(), DateTime.Now, 75.00);
        _mockOrderRepository.Save(_existingOrder).Wait();
        
        _useCase = new DeleteOrder(_mockOrderRepository, _mockLogger);
    }

    [Fact]
    public async Task Execute_WithExistingOrder_RemovesOrderAndLogsSuccess()
    {
        // Arrange
        var input = new DeleteOrderInput(_existingOrderId);
        _mockLogger.Clear(); // Clear setup logs

        // Act
        await _useCase.Execute(input);

        // Assert
        var deletedOrder = await _mockOrderRepository.ById(_existingOrderId);
        Assert.Null(deletedOrder);
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains($"Deleted order with ID {_existingOrderId}", _mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNonExistentOrder_ThrowsElementNotFoundExceptionAndDoesNotLog()
    {
        // Arrange
        var input = new DeleteOrderInput(_nonExistingOrderId);
        _mockLogger.Clear(); 

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => _useCase.Execute(input)
        );
        
        Assert.Contains($"Order with ID {_nonExistingOrderId} not found", exception.Message);
        Assert.Empty(_mockLogger.LoggedMessages);
    }

    [Fact]
    public async Task Execute_WithEmptyGuid_ThrowsElementNotFoundExceptionAndDoesNotLog()
    {
        // Arrange
        var emptyGuid = Guid.Empty;
        var input = new DeleteOrderInput(emptyGuid);
        _mockLogger.Clear();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => _useCase.Execute(input)
        );
        
        Assert.Contains($"Order with ID {emptyGuid} not found", exception.Message);
        Assert.Empty(_mockLogger.LoggedMessages);
    }
}