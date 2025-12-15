using Adria.Application.Supplement;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;
using Xunit;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests.Adria.Application.Supplement;

public sealed class DeleteSupplementTests
{
    private readonly MockSupplementRepository _mockSupplementRepository;
    private readonly MockLogger<CreateSupplement> _mockLogger; 
    private readonly DeleteSupplement _useCase;

    private readonly Guid _supplementIdToDelete = Guid.NewGuid();
    private readonly Guid _nonExistentSupplementId = Guid.NewGuid();

    private readonly global::Adria.Domain.Order.Supplement _existingSupplement;

    public DeleteSupplementTests()
    {
        // Arrange Setup
        _mockSupplementRepository = new MockSupplementRepository();
        _mockLogger = new MockLogger<CreateSupplement>();

        _existingSupplement = new global::Adria.Domain.Order.Supplement(
            _supplementIdToDelete, 
            "Existing Protein", 
            "Protein", 
            50.00, 
            20
        );

        _mockSupplementRepository.Save(_existingSupplement).Wait();
        
        _useCase = new DeleteSupplement(_mockSupplementRepository, _mockLogger);
    }

    [Fact]
    public async Task Execute_WhenSupplementExists_DeletesSupplementAndLogs()
    {
        // Arrange
        var input = new DeleteSupplementInput(_supplementIdToDelete);
        _mockLogger.Clear(); 

        // Act
        await _useCase.Execute(input);

        // Assert 1: Check if the supplement was deleted from the repository
        var deletedSupplement = await _mockSupplementRepository.ById(_supplementIdToDelete);
        Assert.Null(deletedSupplement);

        // Assert 2: Check logging
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains($"Deleted order with ID {_supplementIdToDelete}", _mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WhenSupplementDoesNotExist_ThrowsElementNotFoundExceptionAndDoesNotLog()
    {
        // Arrange
        var input = new DeleteSupplementInput(_nonExistentSupplementId);
        _mockLogger.Clear(); 

        // Act & Assert 1: Expect the ElementNotFoundException
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => _useCase.Execute(input)
        );

        Assert.Contains($"Supplement with ID {_nonExistentSupplementId} not found.", exception.Message);

        // Assert 2: Verify no deletion occurred (by checking the existing one is still there)
        var stillExisting = await _mockSupplementRepository.ById(_supplementIdToDelete);
        Assert.NotNull(stillExisting);
        
        // Assert 3: Check logging (nothing should be logged on validation/not found failure)
        Assert.Empty(_mockLogger.LoggedMessages);
    }

    [Fact]
    public async Task Execute_WithEmptyId_ThrowsElementNotFoundException()
    {
        // Arrange
        var input = new DeleteSupplementInput(Guid.Empty);

        // Act & Assert
        await Assert.ThrowsAsync<ElementNotFoundException>(
            () => _useCase.Execute(input)
        );
    }
}