using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Subcriptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Subscriptions;

public class SearchSubscriptionByIdTests
{
    [Fact]
    public async Task Execute_WithExistingSubscription_ReturnsSubscriptionData()
    {
        // Arrange
        var subscriptionId = Guid.NewGuid();
        var subscriptionData = new SubscriptionData(
            subscriptionId,
            SubscriptionType.Premium,
            19.99,
            "Full access"
        );

        var mockQuery = new MockSubscriptionByIdQuery();
        mockQuery.AddSubscription(subscriptionData);

        var mockLogger = new MockLogger<SearchSubscriptionById>();
        var useCase = new SearchSubscriptionById(mockQuery, mockLogger);
        var input = new SearchSubscriptionByIdInput(subscriptionId);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(subscriptionId, result.SubscriptionId);
        Assert.Equal(SubscriptionType.Premium, result.Type);
        Assert.Equal(19.99, result.PricePerMonth);
        Assert.Equal("Full access", result.Advantages);

        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching subscription with ID {subscriptionId}", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNonExistentSubscription_ThrowsElementNotFoundException()
    {
        // Arrange
        var subscriptionId = Guid.NewGuid();
        var mockQuery = new MockSubscriptionByIdQuery();
        var mockLogger = new MockLogger<SearchSubscriptionById>();
        var useCase = new SearchSubscriptionById(mockQuery, mockLogger);
        var input = new SearchSubscriptionByIdInput(subscriptionId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Contains($"Subscription with ID {subscriptionId} not found", exception.Message);
    }

    [Fact]
    public async Task Execute_WithEmptyGuid_ThrowsElementNotFoundException()
    {
        // Arrange
        var subscriptionId = Guid.Empty;
        var mockQuery = new MockSubscriptionByIdQuery();
        var mockLogger = new MockLogger<SearchSubscriptionById>();
        var useCase = new SearchSubscriptionById(mockQuery, mockLogger);
        var input = new SearchSubscriptionByIdInput(subscriptionId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Contains($"Subscription with ID {subscriptionId} not found", exception.Message);
    }
}