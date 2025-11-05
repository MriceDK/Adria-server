namespace Adria.Domain.Subcriptions;

public interface ISubscriptionRepository
{
    Task<Subscription?> ByType(SubscriptionType subscriptionType);

    Task Save(Subscription subscription);
}