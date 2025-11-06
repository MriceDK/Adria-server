using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Subcriptions;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class AllSubscriptionsQuery : IAllSubscriptionsQuery
{
    private static readonly string QRY = @"
        SELECT id, subscription_type, price_per_month, advantages
        FROM subscriptions
    ";

    private readonly DbProviderFactory _factory;

    private readonly string _connectionString;

    private readonly ILogger<AllSubscriptionsQuery> _logger;

    public AllSubscriptionsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AllSubscriptionsQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<SubscriptionData>> Fetch()
    {
        _logger.LogInformation("Starting to fetch all subscriptions");

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException(
                                   "DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        using var reader = await command.ExecuteReaderAsync();

        var subscriptions = new List<SubscriptionData>();

        while (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("id");
            var typeOrd = reader.GetOrdinal("subscription_type");
            var priceOrd = reader.GetOrdinal("price_per_month");
            var advOrd = reader.GetOrdinal("advantages");

            var id = reader.GetGuid(idOrd);
            var typeString = reader.GetString(typeOrd);
            var typeEnum = (SubscriptionType)Enum.Parse(
                typeof(SubscriptionType),
                typeString,
                ignoreCase: true
            );
            var price = reader.GetDouble(priceOrd);
            var advantages = reader.GetString(advOrd);

            subscriptions.Add(new SubscriptionData(
                id,
                typeEnum,
                price,
                advantages
            ));
        }

        _logger.LogInformation("Fetched {Count} subscriptions", subscriptions.Count);

        return subscriptions.AsReadOnly();
    }
}