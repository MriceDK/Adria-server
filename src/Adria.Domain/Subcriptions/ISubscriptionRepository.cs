namespace Adria.Domain.Subcriptions;

public interface ISubscriptionRepository
{
    Task<Subscription?> ById(Guid subscriptionId);

    Task Save(Subscription subscription);
}