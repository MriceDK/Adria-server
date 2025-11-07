using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class OrderByIdQuery : IOrderByIdQuery
{
    private static readonly string QRY = @"
        SELECT OrderId, AdrianId, Date, TotalPrice
        FROM order
        WHERE id = @Id;
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<OrderByIdQuery> _logger;

    public OrderByIdQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<OrderByIdQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<OrderData?> Fetch(Guid orderId)
    {
        _logger.LogInformation("Starting to fetch order with ID {OrderId}", orderId);

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@Id";
        parameter.Value = orderId.ToString().ToLower();
        command.Parameters.Add(parameter);

        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("OrderId");
            var adrianIdOrd = reader.GetOrdinal("AdrianId");
            var dateOrd = reader.GetOrdinal("Date");
            var totalPriceOrd = reader.GetOrdinal("TotalPrice");

            var id = Guid.Parse(reader.GetString(idOrd));
            var adrianId = Guid.Parse(reader.GetString(adrianIdOrd));
            var date = reader.GetDateTime(dateOrd);
            var totalPrice = reader.GetDouble(totalPriceOrd);

            _logger.LogInformation("Found order with ID {OrderId}", orderId);
            
            return new OrderData(id, adrianId, date, totalPrice);
        }

        _logger.LogInformation("Order with ID {OrderId} not found", orderId);
        return null;
    }
}