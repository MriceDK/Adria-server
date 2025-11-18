using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class OrderByUserIdQuery : IOrderByUserIdQuery
{
    private static readonly string QRY = @"
        SELECT OrderId, AdrianId, Date, TotalPrice
        FROM order
        WHERE AdrianId = @Id;
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<OrderByUserIdQuery> _logger;

    public OrderByUserIdQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<OrderByUserIdQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<OrderData?> Fetch(Guid adrianId)
    {
        _logger.LogInformation("Starting to fetch orders with user ID {AdrianId}", adrianId);

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@Id";
        parameter.Value = adrianId.ToString().ToLower();
        command.Parameters.Add(parameter);

        var orders = new List<OrderData>();
        
        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("OrderId");
            var adrianIdOrd = reader.GetOrdinal("AdrianId");
            var dateOrd = reader.GetOrdinal("Date");
            var totalPriceOrd = reader.GetOrdinal("TotalPrice");

            var orderId = Guid.Parse(reader.GetString(idOrd));
            var id = Guid.Parse(reader.GetString(adrianIdOrd));
            var date = reader.GetDateTime(dateOrd);
            var totalPrice = reader.GetDouble(totalPriceOrd);

            orders.Add(new OrderData(
                orderId,
                id,
                date,
                totalPrice
            ));
            
            _logger.LogInformation("Found order(s) with user ID {AdrianId}", id);
            
            return new OrderData(id, adrianId, date, totalPrice);
        }

        _logger.LogInformation("Orders with user ID {AdrianId} not found", adrianId);
        return null;
    }
}