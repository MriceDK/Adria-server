using Adria.Domain.Subcriptions;

namespace UnitTests.Mocks;

public class MockSubscriptionRepository : ISubscriptionRepository
{
    private readonly Dictionary<Guid, Subscription> _subscriptions = new();

    public Task<Subscription?> ById(Guid subscriptionId)
    {
        _subscriptions.TryGetValue(subscriptionId, out Subscription? subscription);
        return Task.FromResult(subscription);
    }


    public Task Save(Subscription subscription)
    {
        _subscriptions[subscription.Id] = subscription;
        return Task.CompletedTask;
    }

    public void AddSubscription(Subscription subscription)
    {
        _subscriptions[subscription.Id] = subscription;
    }

    public void Clear()
    {
        _subscriptions.Clear();
    }
}