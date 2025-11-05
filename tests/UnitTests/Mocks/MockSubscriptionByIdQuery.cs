using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace UnitTests.Mocks;

public sealed class MockSubscriptionByIdQuery : ISubscriptionByIdQuery
{
    private readonly Dictionary<Guid, SubscriptionData> _subscriptions = new();
    
    public void AddSubscription(SubscriptionData subscription)
    {
        _subscriptions[subscription.SubscriptionId] = subscription;
    }
    public Task<SubscriptionData?> Fetch(Guid subscriptionId)
    {
        _subscriptions.TryGetValue(subscriptionId, out var subscription);
        return Task.FromResult(subscription);
    }
}