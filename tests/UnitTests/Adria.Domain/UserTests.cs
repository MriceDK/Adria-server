using Adria.Domain.Subcriptions;
using Adria.Domain.Users;

namespace UnitTests.Adria.Domain;

public sealed class UserTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesUser()
    {
        // Arrange
        const string name = "John Doe";
        const string job = "Doctor";
        var subscription = new Subscription(SubscriptionType.Basic, 10, "not so much");
        // Act
        var user = new User(name, job, subscription);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(job, user.Job);
        Assert.Equal(subscription.Id, user.Subscription.Id);
        Assert.NotEqual(Guid.Empty, user.AdriaId);
    }

    [Fact]
    public void Constructor_WithValidParametersAndId_CreatesUser()
    {
        // Arrange
        const string name = "John Doe";
        const string job = "Doctor";
        var subscription = new Subscription(SubscriptionType.Basic, 10, "not so much");
        var adrianId = Guid.NewGuid();
        // Act
        var user = new User(name, job, subscription, adrianId);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(job, user.Job);
        Assert.Equal(subscription.Id, user.Subscription.Id);
        Assert.Equal(adrianId, user.AdriaId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        string email = "valid@email.com";
        const string job = "Doctor";
        var subscription = new Subscription(SubscriptionType.Basic, 10, "not so much");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(invalidName, job, subscription));
    }


    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidJob_ThrowsArgumentException(string invalidJob)
    {
        // Arrange
        const string name = "John Doe";
        const string job = "Doctor";
        var subscription = new Subscription(SubscriptionType.Basic, 10, "not so much");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(name, invalidJob, subscription));
    }
}