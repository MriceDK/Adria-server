using Adria.Application.Contracts.Data;
using Adria.Application.Supplement;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Adria.Application.Supplement;

public sealed class SearchAllSupplementsTests
{
    private readonly MockAllSupplementsQuery _mockQuery;
    private readonly MockLogger<SearchAllSupplements> _mockLogger;
    private readonly SearchAllSupplements _useCase;

    private readonly SupplementData _supplement1;
    private readonly SupplementData _supplement2;
    private readonly IReadOnlyCollection<SupplementData?> _validResult;

    public SearchAllSupplementsTests()
    {
        // Arrange Setup
        _mockQuery = new MockAllSupplementsQuery();
        _mockLogger = new MockLogger<SearchAllSupplements>();
        
        _useCase = new SearchAllSupplements(_mockQuery, _mockLogger);

        // Define valid data for success test
        _supplement1 = new SupplementData(Guid.NewGuid(), "Omega-3", "Oil", 25.00, 50);
        _supplement2 = new SupplementData(Guid.NewGuid(), "Creatine", "Powder", 40.00, 75);
        _validResult = new List<SupplementData?> { _supplement1, _supplement2 }.AsReadOnly();
    }

    [Fact]
    public async Task Execute_WhenSupplementsExist_ReturnsCorrectCollection()
    {
        // Arrange
        _mockQuery.SetReturnValue(_validResult);
        _mockLogger.Clear(); 

        // Act
        var result = await _useCase.Execute();

        // Assert 1: Check return value
        Assert.Equal(2, result.Count);
        Assert.Contains(_supplement1, result);
        Assert.Contains(_supplement2, result);
        
        // Assert 2: Check logging
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains("Fetching all supplements...", _mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WhenSupplementsAreEmpty_ReturnsEmptyCollection()
    {
        // Arrange
        _mockQuery.SetReturnValue(new List<SupplementData?>().AsReadOnly());
        _mockLogger.Clear(); 

        // Act
        var result = await _useCase.Execute();

        // Assert 1: Check return value
        Assert.Empty(result);
        
        // Assert 2: Check logging
        Assert.Single(_mockLogger.LoggedMessages);
        Assert.Contains("Fetching all supplements...", _mockLogger.LoggedMessages[0]);
    }

}