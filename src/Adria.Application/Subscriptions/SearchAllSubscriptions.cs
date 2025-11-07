using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Subscriptions;

public sealed class SearchAllSubscriptions : IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>
{
    private readonly IAllSubscriptionsQuery _allSubscriptionsQuery;
    private readonly ILogger<SearchAllSubscriptions> _logger;

    public SearchAllSubscriptions(IAllSubscriptionsQuery allSubscriptionsQuery, ILogger<SearchAllSubscriptions> logger)
    {
        _allSubscriptionsQuery = allSubscriptionsQuery;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<SubscriptionData>> Execute()
    {
        _logger.LogInformation("Fetching all subcriptions");

        return await _allSubscriptionsQuery.Fetch();
    }
}