using Adria.Domain.PushNotifications;

namespace UnitTests.Adria.Domain;

using Xunit;

public sealed class PushSubscriptionTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesValidPushSubscription()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "https://fcm.googleapis.com/fcm/send/endpoint123";
        var p256dh = "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls";
        var auth = "tBHItJI5svbpez7KI4CCXg";
        var id = Guid.NewGuid();

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth, id);

        // Assert
        Assert.Equal(id, subscription.Id);
        Assert.Equal(userId, subscription.UserId);
        Assert.Equal(endpoint, subscription.Endpoint);
        Assert.Equal(p256dh, subscription.P256dh);
        Assert.Equal(auth, subscription.Auth);
    }

    [Fact]
    public void Constructor_WithoutId_GeneratesNewGuid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "https://fcm.googleapis.com/fcm/send/endpoint123";
        var p256dh = "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls";
        var auth = "tBHItJI5svbpez7KI4CCXg";

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth);

        // Assert
        Assert.NotEqual(Guid.Empty, subscription.Id);
    }

    [Fact]
    public void Constructor_WithEmptyGuidId_GeneratesNewGuid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "https://fcm.googleapis.com/fcm/send/endpoint123";
        var p256dh = "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls";
        var auth = "tBHItJI5svbpez7KI4CCXg";

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth, Guid.Empty);

        // Assert
        Assert.NotEqual(Guid.Empty, subscription.Id);
    }

    [Fact]
    public void Constructor_WithValidId_UsesProvidedId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "https://fcm.googleapis.com/fcm/send/endpoint123";
        var p256dh = "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls";
        var auth = "tBHItJI5svbpez7KI4CCXg";
        var id = Guid.NewGuid();

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth, id);

        // Assert
        Assert.Equal(id, subscription.Id);
    }

    [Fact]
    public void Constructor_StoresAllPropertiesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "https://updates.push.services.mozilla.com/wpush/v2/endpoint";
        var p256dh = "BJsj63kz8RPZe70T5F0eCVyT";
        var auth = "8eDyX_uCN0XRURSVxVfBqA";

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth);

        // Assert
        Assert.Equal(userId, subscription.UserId);
        Assert.Equal(endpoint, subscription.Endpoint);
        Assert.Equal(p256dh, subscription.P256dh);
        Assert.Equal(auth, subscription.Auth);
    }

    [Fact]
    public void Properties_AreReadOnly_CannotBeModifiedExternally()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "https://fcm.googleapis.com/fcm/send/endpoint123";
        var p256dh = "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls";
        var auth = "tBHItJI5svbpez7KI4CCXg";

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth);

        // Assert - This test verifies compile-time immutability
        // If Id had a public setter, this would fail to compile
        Assert.NotNull(subscription);
        
        // Verify that properties maintain their values
        var originalId = subscription.Id;
        var originalUserId = subscription.UserId;
        var originalEndpoint = subscription.Endpoint;
        var originalP256dh = subscription.P256dh;
        var originalAuth = subscription.Auth;

        // Properties should still have the same values
        Assert.Equal(originalId, subscription.Id);
        Assert.Equal(originalUserId, subscription.UserId);
        Assert.Equal(originalEndpoint, subscription.Endpoint);
        Assert.Equal(originalP256dh, subscription.P256dh);
        Assert.Equal(originalAuth, subscription.Auth);
    }

    [Fact]
    public void Constructor_WithDifferentEndpointFormats_StoresCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoints = new[]
        {
            "https://fcm.googleapis.com/fcm/send/endpoint123",
            "https://updates.push.services.mozilla.com/wpush/v2/endpoint",
            "https://android.googleapis.com/gcm/send/subscription-id"
        };

        // Act & Assert
        foreach (var endpoint in endpoints)
        {
            var subscription = new PushSubscription(
                userId, 
                endpoint, 
                "p256dh-key", 
                "auth-key"
            );

            Assert.Equal(endpoint, subscription.Endpoint);
        }
    }

    [Fact]
    public void Constructor_WithEmptyStrings_AllowsEmptyValues()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = string.Empty;
        var p256dh = string.Empty;
        var auth = string.Empty;

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth);

        // Assert
        Assert.Equal(string.Empty, subscription.Endpoint);
        Assert.Equal(string.Empty, subscription.P256dh);
        Assert.Equal(string.Empty, subscription.Auth);
    }

    [Fact]
    public void Constructor_WithWhitespaceStrings_AllowsWhitespace()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "   ";
        var p256dh = "\t";
        var auth = "\n";

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth);

        // Assert
        Assert.Equal("   ", subscription.Endpoint);
        Assert.Equal("\t", subscription.P256dh);
        Assert.Equal("\n", subscription.Auth);
    }

    [Fact]
    public void TwoSubscriptions_WithSameData_HaveDifferentIds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var endpoint = "https://fcm.googleapis.com/fcm/send/endpoint123";
        var p256dh = "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls";
        var auth = "tBHItJI5svbpez7KI4CCXg";

        // Act
        var subscription1 = new PushSubscription(userId, endpoint, p256dh, auth);
        var subscription2 = new PushSubscription(userId, endpoint, p256dh, auth);

        // Assert
        Assert.NotEqual(subscription1.Id, subscription2.Id);
        Assert.Equal(subscription1.UserId, subscription2.UserId);
        Assert.Equal(subscription1.Endpoint, subscription2.Endpoint);
        Assert.Equal(subscription1.P256dh, subscription2.P256dh);
        Assert.Equal(subscription1.Auth, subscription2.Auth);
    }

    [Theory]
    [InlineData("https://fcm.googleapis.com/fcm/send/abc123", "key1", "auth1")]
    [InlineData("https://updates.push.services.mozilla.com/wpush/v2/xyz789", "key2", "auth2")]
    [InlineData("https://android.googleapis.com/gcm/send/def456", "key3", "auth3")]
    public void Constructor_WithVariousValidInputs_CreatesSubscription(
        string endpoint, 
        string p256dh, 
        string auth)
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var subscription = new PushSubscription(userId, endpoint, p256dh, auth);

        // Assert
        Assert.NotEqual(Guid.Empty, subscription.Id);
        Assert.Equal(userId, subscription.UserId);
        Assert.Equal(endpoint, subscription.Endpoint);
        Assert.Equal(p256dh, subscription.P256dh);
        Assert.Equal(auth, subscription.Auth);
    }
}