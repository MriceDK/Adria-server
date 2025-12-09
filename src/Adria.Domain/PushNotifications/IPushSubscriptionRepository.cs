namespace Adria.Domain.PushNotifications;

public interface IPushSubscriptionRepository
{
    Task Save(PushSubscription subscription);
    Task<List<PushSubscription>> GetAll();
    Task Delete(Guid id); 
}