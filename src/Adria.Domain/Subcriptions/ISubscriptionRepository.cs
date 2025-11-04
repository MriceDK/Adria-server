namespace Adria.Domain.Subcriptions;

public interface ISubscriptionRepository
{
    Task<Subscription?> ByType(Type type);

    Task Save(Subscription subscription);
}