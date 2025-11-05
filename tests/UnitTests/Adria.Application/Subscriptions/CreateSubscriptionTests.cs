using Adria.Application.Subscriptions;
using Adria.Domain.Subcriptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Subscriptions;

public class CreateSubscriptionTests
{
     [Fact]
    public async Task Execute_WithValidInput_CreatesSubscriptionAndReturnsId()
    {
        // Arrange
        var mockRepository = new MockSubscriptionRepository();
        var mockLogger = new MockLogger<CreateSubscription>();
        var useCase = new CreateSubscription(mockRepository, mockLogger);
        var input = new CreateSubscriptionInput(
            SubscriptionType.Basic,
            9.99,
            "Access to basic features"
        );

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotEqual(Guid.Empty, result);

        var savedSubscription = await mockRepository.ById(result);
        Assert.NotNull(savedSubscription);
        Assert.Equal(SubscriptionType.Basic, savedSubscription.SubscriptionType);
        Assert.Equal(9.99, savedSubscription.PricePerMonth);
        Assert.Equal("Access to basic features", savedSubscription.Advantages);

        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("New subscription created", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithEmptyAdvantages_ThrowsArgumentException()
    {
        // Arrange
        var mockRepository = new MockSubscriptionRepository();
        var mockLogger = new MockLogger<CreateSubscription>();
        var useCase = new CreateSubscription(mockRepository, mockLogger);
        var input = new CreateSubscriptionInput(
            SubscriptionType.Standard,
            19.99,
            ""
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WithNullAdvantages_ThrowsArgumentException()
    {
        // Arrange
        var mockRepository = new MockSubscriptionRepository();
        var mockLogger = new MockLogger<CreateSubscription>();
        var useCase = new CreateSubscription(mockRepository, mockLogger);
        var input = new CreateSubscriptionInput(
            SubscriptionType.Premium,
            29.99,
            null!
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
    }
}