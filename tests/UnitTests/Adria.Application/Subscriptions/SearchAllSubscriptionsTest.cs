using Adria.Application.Subscriptions;
using Adria.Application.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adria.Domain.Subcriptions;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application.Subscriptions;

public sealed class SearchAllSubscriptionsTest
{
    [Fact]
    public async Task Execute_ReturnsAllSubscriptions()
    {
        var query = new MockAllSubscriptionsQuery();
        var logger = new MockLogger<SearchAllSubscriptions>();

        var subscriptions = new[]
        {
            new SubscriptionData(Guid.NewGuid(), SubscriptionType.Basic, 10,"basic"),
            new SubscriptionData(Guid.NewGuid(), SubscriptionType.Premium, 20,"premium")
        };

        query.SetReturnValue(subscriptions);

        var useCase = new SearchAllSubscriptions(query, logger);

        var result = await useCase.Execute();

        Assert.Equal(2, result.Count);
        Assert.Equal(subscriptions, result);
        Assert.Single(logger.LoggedMessages);
        Assert.Contains("Fetching all subcriptions", logger.LoggedMessages[0]);
    }
}