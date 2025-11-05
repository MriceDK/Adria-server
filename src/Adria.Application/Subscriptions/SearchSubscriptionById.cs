using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Subscriptions;

public sealed record SearchSubscriptionByIdInput(
    Guid SubscriptionId
);

public sealed class SearchSubscriptionById : IUseCase<SearchSubscriptionByIdInput, Task<SubscriptionData>>
{
    private readonly ISubscriptionByIdQuery _subscriptionByIdQuery;
    private readonly ILogger<SearchSubscriptionById> _logger;

    public SearchSubscriptionById(
        ISubscriptionByIdQuery subscriptionByIdQuery,
        ILogger<SearchSubscriptionById> logger
    )
    {
        _subscriptionByIdQuery = subscriptionByIdQuery;
        _logger = logger;
    }

    public async Task<SubscriptionData> Execute(SearchSubscriptionByIdInput input)
    {
        _logger.LogInformation(
            "Fetching subscription with ID {SubscriptionId}",
            input.SubscriptionId
        );

        return (await _subscriptionByIdQuery.Fetch(input.SubscriptionId))
               ?? throw new ElementNotFoundException(
                   $"Subscription with ID {input.SubscriptionId} not found."
               );
    }
}