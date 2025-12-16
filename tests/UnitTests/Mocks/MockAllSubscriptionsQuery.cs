using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockAllSubscriptionsQuery : IAllSubscriptionsQuery
{
    private IReadOnlyCollection<SubscriptionData> _returnValue =
        Array.Empty<SubscriptionData>();

    public void SetReturnValue(IReadOnlyCollection<SubscriptionData> value)
    {
        _returnValue = value;
    }

    public Task<IReadOnlyCollection<SubscriptionData>> Fetch()
    {
        return Task.FromResult(_returnValue);
    }
}