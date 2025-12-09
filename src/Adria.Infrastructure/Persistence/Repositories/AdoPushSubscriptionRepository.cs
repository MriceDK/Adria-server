using System.Data.Common;
using Adria.Domain.PushNotifications;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoPushSubscriptionRepository : AbstractAdoRepository, IPushSubscriptionRepository
{
    private readonly ILogger<AdoPushSubscriptionRepository> _logger;

    private const string INSERT_SQL = @"
        INSERT INTO pushSubscriptions (SubscriptionId, UserId, Endpoint, P256dh, Auth)
        VALUES (@Id, @UserId, @Endpoint, @P256dh, @Auth);
    ";

    public AdoPushSubscriptionRepository(
        DbProviderFactory factory, 
        string connectionString,
        ILogger<AdoPushSubscriptionRepository> logger) 
        : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task Save(PushSubscription subscription)
    {
        var parameters = new[]
        {
            CreateParameter("@Id", subscription.Id.ToString().ToLower()),
            CreateParameter("@UserId", subscription.UserId.ToString().ToLower()),
            CreateParameter("@Endpoint", subscription.Endpoint),
            CreateParameter("@P256dh", subscription.P256dh),
            CreateParameter("@Auth", subscription.Auth)
        };

        await ExecuteNonQueryAsync(INSERT_SQL, parameters);
        _logger.LogInformation("Saved push subscription {Id}", subscription.Id);
    }
}