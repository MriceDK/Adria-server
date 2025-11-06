using System.Data.Common;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;
using Microsoft.Extensions.Logging;


namespace Adria.Infrastructure.Persistence.Queries;

public class AllOrdersQuery
{
    private static readonly string QRY = @"
        SELECT id, adrian_id, `date`, total_price
        FROM orders
    ";

    private readonly DbProviderFactory _factory;

    private readonly string _connectionString;

    private readonly ILogger<AllOrdersQuery> _logger;

    public AllOrdersQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AllOrdersQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<OrderData>> Fetch()
    {
        _logger.LogInformation("Starting to fetch all orders");

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException(
                                   "DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        using var reader = await command.ExecuteReaderAsync();

        var orders = new List<OrderData>();

        while (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("id");
            var adrianIdOrd = reader.GetOrdinal("adrian_id");
            var dateOrd = reader.GetOrdinal("date");
            var totalPriceOrd = reader.GetOrdinal("total_price");

            var id = reader.GetGuid(idOrd);
            var adrianId = reader.GetGuid(adrianIdOrd);
            var date = reader.GetDateTime(dateOrd);
            var totalPrice = reader.GetDouble(totalPriceOrd);

            orders.Add(new OrderData(
                id,
                adrianId,
                date,
                totalPrice
            ));
        }

        _logger.LogInformation("Fetched {Count} orders", orders.Count);

        return orders.AsReadOnly();
    }
}