using Adria.Domain.Subcriptions;
using Type = Adria.Domain.Subcriptions.Type;

namespace UnitTests.Adria.Domain;

public sealed class SubscriptionTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesSubscription()
    {
        // Arrange
        var type = Type.Basic;
        const double price = 9.99;
        const string advantages = "Basic advantages";

        // Act
        var subscription = new Subscription(type, price, advantages);

        // Assert
        Assert.Equal(type, subscription.Type);
        Assert.Equal(price, subscription.PricePerMonth);
        Assert.Equal(advantages, subscription.Advantages);
        Assert.NotEqual(Guid.Empty, subscription.Id);
    }

    [Fact]
    public void Constructor_WithValidParametersAndId_CreatesSubscription()
    {
        // Arrange
        var type = Type.Premium;
        const double price = 19.99;
        const string advantages = "Premium advantages";
        var subscriptionId = Guid.NewGuid();

        // Act
        var subscription = new Subscription(type, price, advantages, subscriptionId);

        // Assert
        Assert.Equal(type, subscription.Type);
        Assert.Equal(price, subscription.PricePerMonth);
        Assert.Equal(advantages, subscription.Advantages);
        Assert.Equal(subscriptionId, subscription.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidAdvantages_ThrowsArgumentException(string invalidAdvantages)
    {
        // Arrange
        var type = Type.Basic;
        const double price = 10;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Subscription(type, price, invalidAdvantages));

        Assert.Equal("advantages", exception.ParamName);
    }

    [Fact]
    public void Properties_CanBeSet_AfterConstruction()
    {
        // Arrange
        var subscription = new Subscription(Type.Basic, 10, "Initial advantages");

        var newType = Type.Premium;
        const double newPrice = 25.50;
        const string newAdvantages = "New advantages";

        // Act
        subscription.Type = newType;
        subscription.PricePerMonth = newPrice;
        subscription.Advantages = newAdvantages;

        // Assert
        Assert.Equal(newType, subscription.Type);
        Assert.Equal(newPrice, subscription.PricePerMonth);
        Assert.Equal(newAdvantages, subscription.Advantages);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Advantages_Setter_AllowsInvalidValues_AfterConstruction(string invalidAdvantages)
    {
        // Arrange
        var subscription = new Subscription(Type.Basic, 10, "Valid advantages");

        // Act
        // Note: The validation is only in the constructor, not the setter.
        // This test confirms that behavior.
        subscription.Advantages = invalidAdvantages;

        // Assert
        Assert.Equal(invalidAdvantages, subscription.Advantages);
    }
}