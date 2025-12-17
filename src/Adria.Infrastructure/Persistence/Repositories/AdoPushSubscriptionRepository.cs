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
    private const string SELECT_ALL = "SELECT SubscriptionId, UserId, Endpoint, P256dh, Auth FROM pushSubscriptions";
    private const string DELETE_SQL = "DELETE FROM pushSubscriptions WHERE SubscriptionId = @Id";

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
    public async Task<List<PushSubscription>> GetAll()
    {
        var list = new List<PushSubscription>();
        using var connection = _factory.CreateConnection()!;
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = SELECT_ALL;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new PushSubscription(
                Guid.Parse(reader.GetString(reader.GetOrdinal("UserId"))),
                reader.GetString(reader.GetOrdinal("Endpoint")),
                reader.GetString(reader.GetOrdinal("P256dh")),
                reader.GetString(reader.GetOrdinal("Auth")),
                Guid.Parse(reader.GetString(reader.GetOrdinal("SubscriptionId")))
            ));
        }
        await connection.CloseAsync();
        return list;
    }

    public async Task Delete(Guid id)
    {
        var param = CreateParameter("@Id", id.ToString().ToLower());
        await ExecuteNonQueryAsync(DELETE_SQL, [param]);
    }
}