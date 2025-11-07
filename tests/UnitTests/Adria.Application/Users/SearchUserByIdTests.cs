using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Subcriptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public sealed class SearchUserByIdTests
{
    [Fact]
    public async Task Execute_WithExistingUser_ReturnsUserData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userData = new UserData(
            userId, 
            "John Doe", 
            "Software Developer", 
            nameof(SubscriptionType.Basic)
        );
        
        var mockQuery = new MockUserByIdQuery();
        mockQuery.AddUser(userData);
        
        var mockLogger = new MockLogger<SearchUserById>();
        var useCase = new SearchUserById(mockQuery, mockLogger);
        var input = new SearchUserByIdInput(userId);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.AdriaId);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("Software Developer", result.Job);
        Assert.Equal(nameof(SubscriptionType.Basic), result.Subscription);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching user with ID {userId}", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNonExistentUser_ThrowsElementNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockQuery = new MockUserByIdQuery();
        var mockLogger = new MockLogger<SearchUserById>();
        var useCase = new SearchUserById(mockQuery, mockLogger);
        var input = new SearchUserByIdInput(userId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => useCase.Execute(input)
        );
        
        Assert.Contains($"User with ID {userId} not found", exception.Message);
    }

    [Fact]
    public async Task Execute_WithEmptyGuid_ThrowsElementNotFoundException()
    {
        // Arrange
        var userId = Guid.Empty;
        var mockQuery = new MockUserByIdQuery();
        var mockLogger = new MockLogger<SearchUserById>();
        var useCase = new SearchUserById(mockQuery, mockLogger);
        var input = new SearchUserByIdInput(userId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(
            () => useCase.Execute(input)
        );
        
        Assert.Contains($"User with ID {userId} not found", exception.Message);
    }
}