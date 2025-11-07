using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class AllOrdersQuery : IAllOrdersQuery
{
    private static readonly string QRY = @"
        SELECT OrderId, AdrianId, Date, TotalPrice
        FROM order
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
            var idOrd = reader.GetOrdinal("OrderId");
            var adrianIdOrd = reader.GetOrdinal("AdrianId");
            var dateOrd = reader.GetOrdinal("Date");
            var totalPriceOrd = reader.GetOrdinal("TotalPrice");

            var id = Guid.Parse(reader.GetString(idOrd));
            var adrianId = Guid.Parse(reader.GetString(adrianIdOrd));
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