using Adria.Domain.PushNotifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockPushSubscriptionRepository : IPushSubscriptionRepository
{
    public List<PushSubscription> SavedEntities { get; } = new();

    public Task Save(PushSubscription subscription)
    {
        SavedEntities.Add(subscription);
        return Task.CompletedTask;
    }

    public Task<List<PushSubscription>> GetAll()
    {
        return Task.FromResult(SavedEntities);
    }

    public Task Delete(Guid id)
    {
        SavedEntities.RemoveAll(s => s.Id == id);
        return Task.CompletedTask;
    }
}