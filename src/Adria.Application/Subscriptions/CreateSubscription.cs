using Adria.Application.Contracts;
using Adria.Domain.Subcriptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Subscriptions;

public sealed record CreateSubscriptionInput(
    SubscriptionType Type,
    double PricePerMonth,
    string Advantages
);

public sealed class CreateSubscription(
    ISubscriptionRepository subscriptionRepository,
    ILogger<CreateSubscription> logger)
    : IUseCase<CreateSubscriptionInput, Task<Guid>>
{
    private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
    private readonly ILogger<CreateSubscription> _logger = logger;

    public async Task<Guid> Execute(CreateSubscriptionInput input)
    {
        Subscription subscription = new(input.Type, input.PricePerMonth, input.Advantages);

        await _subscriptionRepository.Save(subscription);

        _logger.LogInformation(
            "New subscription created with ID {SubscriptionId}, Type: {Type}, Price: {Price}",
            subscription.Id,
            subscription.SubscriptionType,
            subscription.PricePerMonth
        );

        return subscription.Id;
    }
}