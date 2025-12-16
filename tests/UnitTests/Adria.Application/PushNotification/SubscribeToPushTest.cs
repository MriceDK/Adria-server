using Adria.Application.PushNotifications;
using Adria.Domain.PushNotifications;
using System;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application.PushNotification;

public sealed class SubscribeToPushTest
{
    [Fact]
    public async Task Execute_WithValidInput_SavesSubscription()
    {
        var repository = new MockPushSubscriptionRepository();
        var logger = new MockLogger<SubscribeToPush>();

        var useCase = new SubscribeToPush(repository, logger);

        var userId = Guid.NewGuid();
        var input = new SubscribeToPushInput(
            userId,
            "https://endpoint.test",
            new PushSubscriptionKeys("p256dh-key", "auth-key")
        );

        var exception = await Record.ExceptionAsync(() => useCase.Execute(input));

        Assert.Null(exception);
        Assert.Single(repository.SavedEntities);

        var saved = repository.SavedEntities[0];
        Assert.Equal(userId, saved.UserId);
        Assert.Equal("https://endpoint.test", saved.Endpoint);
        Assert.Equal("p256dh-key", saved.P256dh);
        Assert.Equal("auth-key", saved.Auth);

        Assert.Single(logger.LoggedMessages);
        Assert.Contains("Push subscription saved for user", logger.LoggedMessages[0]);
    }
}