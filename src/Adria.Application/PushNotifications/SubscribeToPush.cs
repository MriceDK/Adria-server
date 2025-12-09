using Adria.Application.Contracts;
using Adria.Domain.PushNotifications;
using Microsoft.Extensions.Logging;

namespace Adria.Application.PushNotifications;

public sealed record SubscribeToPushInput(
    Guid UserId,
    string Endpoint,
    PushSubscriptionKeys Keys
);

public sealed record PushSubscriptionKeys(
    string P256dh,
    string Auth
);

public sealed class SubscribeToPush : IUseCase<SubscribeToPushInput, Task>
{
    private readonly IPushSubscriptionRepository _repository;
    private readonly ILogger<SubscribeToPush> _logger;

    public SubscribeToPush(IPushSubscriptionRepository repository, ILogger<SubscribeToPush> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Execute(SubscribeToPushInput input)
    {
        var subscription = new PushSubscription(
            input.UserId,
            input.Endpoint,
            input.Keys.P256dh,
            input.Keys.Auth
        );

        await _repository.Save(subscription);
        
        _logger.LogInformation("Push subscription saved for user {UserId}", input.UserId);
    }
}