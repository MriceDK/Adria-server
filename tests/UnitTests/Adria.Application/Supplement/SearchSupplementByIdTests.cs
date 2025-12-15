using Adria.Application.Contracts.Data;
using Adria.Application.Supplement;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;
using Xunit;
using System;
using System.Threading.Tasks;

namespace UnitTests.Adria.Application.Supplement;

public sealed class SearchSupplementsByIdTests
{
    private readonly MockSupplementsByIdQuery _mockQuery;
    private readonly MockLogger<SearchSupplementsById> _mockLogger;
    private readonly SearchSupplementsById _useCase;

    private readonly Guid _validId = Guid.NewGuid();
    private readonly Guid _nonExistentId = Guid.NewGuid();

    private readonly SupplementData _validSupplementData;

    public SearchSupplementsByIdTests()
    {
        _mockQuery = new MockSupplementsByIdQuery();
        _mockLogger = new MockLogger<SearchSupplementsById>();
        
        _useCase = new SearchSupplementsById(_mockQuery, _mockLogger);

        _validSupplementData = new SupplementData(_validId, "Vitamin D3", "Vitamin", 15.00, 100);
    }

    [Fact]
    public async Task Execute_WhenSupplementExists_ReturnsCorrectData()
    {
        var input = new SearchSupplementsByIdInput(_validId);
        _mockQuery.SetReturnValue(_validSupplementData);
        _mockLogger.Clear(); 

        var result = await _useCase.Execute(input);

        Assert.NotNull(result);
        Assert.Equal(_validId, result!.SupplementId);
        
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains($"Fetching supplements with id '{_validId}'.", _mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WhenSupplementDoesNotExist_ThrowsElementNotFoundException()
    {
        var input = new SearchSupplementsByIdInput(_nonExistentId);
        _mockQuery.SetReturnValue(null);
        _mockLogger.Clear(); 

        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => _useCase.Execute(input)
        );

        Assert.Equal($"Supplement with id {_nonExistentId} not found.", exception.Message);

        Assert.Single(_mockLogger.LoggedMessages);
    }
    
    [Fact]
    public async Task Execute_WithEmptyId_ThrowsElementNotFoundException()
    {
        var input = new SearchSupplementsByIdInput(Guid.Empty);
        _mockQuery.SetReturnValue(null);
        _mockLogger.Clear(); 

        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => _useCase.Execute(input)
        );

        Assert.Equal($"Supplement with id {Guid.Empty} not found.", exception.Message);
        Assert.Single(_mockLogger.LoggedMessages);
    }
}