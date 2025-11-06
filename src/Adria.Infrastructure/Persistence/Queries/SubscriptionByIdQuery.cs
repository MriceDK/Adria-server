using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Subcriptions;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class SubscriptionByIdQuery : ISubscriptionByIdQuery
{
    private static readonly string QRY = @"
        SELECT id, subscription_type, price_per_month, advantages
        FROM subscriptions
        WHERE id = @Id;
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<SubscriptionByIdQuery> _logger;

    public SubscriptionByIdQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<SubscriptionByIdQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<SubscriptionData?> Fetch(Guid subscriptionId)
    {
        _logger.LogInformation("Starting to fetch subscription with ID {SubscriptionId}", subscriptionId);

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@Id";
        parameter.Value = subscriptionId.ToString().ToLower();
        command.Parameters.Add(parameter);

        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("id");
            var typeOrd = reader.GetOrdinal("subscription_type");
            var priceOrd = reader.GetOrdinal("price_per_month");
            var advOrd = reader.GetOrdinal("advantages");

            var id = Guid.Parse(reader.GetString(idOrd));
            var type = reader.GetString(typeOrd);
            var price = reader.GetDouble(priceOrd);
            var advantages = reader.GetString(advOrd);

            _logger.LogInformation("Found subscription with ID {SubscriptionId}", subscriptionId);
            
            return new SubscriptionData(id,Enum.Parse<SubscriptionType>(type), price, advantages);
        }

        _logger.LogInformation("Subscription with ID {SubscriptionId} not found", subscriptionId);
        return null;
    }
}