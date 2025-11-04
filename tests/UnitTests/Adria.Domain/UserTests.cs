using Adria.Domain.Subcriptions;
using Adria.Domain.Users;
using Type = Adria.Domain.Subcriptions.Type;

namespace UnitTests.Adria.Domain;

public sealed class UserTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesUser()
    {
        // Arrange
        const string name = "John Doe";
        const string job = "Doctor";
        var subscription = new Subscription(Type.Basic, 10, "not so much");
        // Act
        var user = new User(name, job, subscription);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(job, user.Job);
        Assert.Equal(subscription.SubscriptionId, user.Subscription.SubscriptionId);
        Assert.NotEqual(Guid.Empty, user.AdrianId);
    }

    [Fact]
    public void Constructor_WithValidParametersAndId_CreatesUser()
    {
        // Arrange
        const string name = "John Doe";
        const string job = "Doctor";
        var subscription = new Subscription(Type.Basic, 10, "not so much");
        var adrianId = Guid.NewGuid();
        // Act
        var user = new User(name, job, subscription, adrianId);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(job, user.Job);
        Assert.Equal(subscription.SubscriptionId, user.Subscription.SubscriptionId);
        Assert.Equal(adrianId, user.AdrianId);
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
        var subscription = new Subscription(Type.Basic, 10, "not so much");

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
        var subscription = new Subscription(Type.Basic, 10, "not so much");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(name, invalidJob, subscription));
    }
}