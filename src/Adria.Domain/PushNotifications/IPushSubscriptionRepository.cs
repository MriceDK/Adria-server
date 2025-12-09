namespace Adria.Domain.PushNotifications;

public interface IPushSubscriptionRepository
{
    Task Save(PushSubscription subscription);
}