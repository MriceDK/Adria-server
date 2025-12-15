using Adria.Application.Order;
using UnitTests.Mocks;
using Xunit;
using System;
using System.Threading.Tasks;

namespace UnitTests.Adria.Application.Order;

public sealed class CreateOrderTests
{
    private readonly MockOrderRepository _mockOrderRepository;
    private readonly MockLogger<CreateOrder> _mockLogger;
    private readonly CreateOrder _useCase;

    private readonly Guid _validOrderId = Guid.NewGuid();
    private readonly Guid _validAdrianId = Guid.NewGuid();
    private readonly DateTime _validDate = new DateTime(2025, 12, 15);
    private const double _validPrice = 19.99;

    public CreateOrderTests()
    {
        _mockOrderRepository = new MockOrderRepository();
        _mockLogger = new MockLogger<CreateOrder>();
        
        _useCase = new CreateOrder(_mockOrderRepository, _mockLogger);
    }

    [Fact]
    public async Task Execute_WithValidInput_CreatesOrderAndReturnsId()
    {
        // Arrange
        var input = new CreateOrderInput(
            _validOrderId, 
            _validAdrianId, 
            _validDate, 
            _validPrice
        );

        // Act
        var resultId = await _useCase.Execute(input);

        // Assert 1
        Assert.Equal(_validOrderId, resultId);
        
        var savedOrder = await _mockOrderRepository.ById(resultId);
        Assert.NotNull(savedOrder);
        Assert.Equal(_validAdrianId, savedOrder.AdrianId);
        Assert.Equal(_validPrice, savedOrder.TotalPrice);
        
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains("New order created with ID", _mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNegativeTotalPrice_ThrowsArgumentExceptionAndDoesNotSave()
    {
        // Arrange
        const double invalidPrice = -5.00;
        var input = new CreateOrderInput(
            _validOrderId, 
            _validAdrianId, 
            _validDate, 
            invalidPrice
        );

        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.Execute(input));
        
        var savedOrder = await _mockOrderRepository.ById(_validOrderId);
        Assert.Null(savedOrder);
        
        Assert.Empty(_mockLogger.LoggedMessages);
    }
}