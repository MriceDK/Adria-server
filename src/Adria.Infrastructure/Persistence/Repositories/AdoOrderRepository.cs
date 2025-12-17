using System.Data.Common;
using Adria.Domain.Order;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoOrderRepository : AbstractAdoRepository, IOrderRepository
{
    private readonly ILogger<AdoOrderRepository> _logger;
    private static readonly string TABLE_ORDERS = "orders";
    private static readonly string COL_ID = "OrderId";
    private static readonly string COL_ADRIAN_ID = "AdrianId";
    private static readonly string COL_DATE = "Date";
    private static readonly string COL_TOTAL_PRICE = "TotalPrice";

    private static readonly string INSERT_ORDER = $@"
        INSERT INTO {TABLE_ORDERS} ({COL_ID}, {COL_ADRIAN_ID}, {COL_DATE}, {COL_TOTAL_PRICE})
        VALUES (@Id, @AdrianId, @Date, @TotalPrice);
    ";

    private static readonly string UPDATE_ORDER = $@"
        UPDATE {TABLE_ORDERS}
        SET {COL_ADRIAN_ID} = @AdrianId, 
            {COL_DATE} = @Date, 
            {COL_TOTAL_PRICE} = @TotalPrice
        WHERE {COL_ID} = @Id;
    ";

    private static readonly string SELECT_ORDER_BY_ID = $@"
        SELECT {COL_ID}, {COL_ADRIAN_ID}, {COL_DATE}, {COL_TOTAL_PRICE}
        FROM {TABLE_ORDERS}
        WHERE {COL_ID} = @Id;
    ";

    private static readonly string SELECT_ORDERS_BY_USER_ID = $@"
        SELECT {COL_ID}, {COL_ADRIAN_ID}, {COL_DATE}, {COL_TOTAL_PRICE}
        FROM {TABLE_ORDERS}
        WHERE {COL_ADRIAN_ID} = @AdrianId;
    ";

    private static readonly string DELETE_ORDER = $@"
        DELETE FROM {TABLE_ORDERS}
        WHERE {COL_ID} = @Id;
    ";

    private static readonly string SELECT_ALL_ORDERS = $@"
        SELECT {COL_ID}, {COL_ADRIAN_ID}, {COL_DATE}, {COL_TOTAL_PRICE}
        FROM {TABLE_ORDERS};
    ";

    public AdoOrderRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoOrderRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Order>> GetAll()
    {
        _logger.LogInformation("Fetching all orders from the database.");

        try
        {
            var orders = new List<Order>();

            using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException("Could not create DB connection.");
            connection.ConnectionString = _connectionString;
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = SELECT_ALL_ORDERS;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var order = new Order(
                    reader.GetGuid(reader.GetOrdinal(COL_ID)),
                    reader.GetGuid(reader.GetOrdinal(COL_ADRIAN_ID)),
                    reader.GetDateTime(reader.GetOrdinal(COL_DATE)),
                    reader.GetDouble(reader.GetOrdinal(COL_TOTAL_PRICE))
                );

                orders.Add(order);
            }
            await connection.CloseAsync();
            return orders;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to retrieve all orders from the database.");
            throw new NutriscanDatabaseException("Failed to retrieve orders from the database.", ex);
        }
    }

    public async Task<Order?> ById(Guid orderId)
    {
        DbParameter id = CreateParameter("@Id", orderId.ToString().ToLower());
        var dbDataReader = await ExecuteReaderAsync(SELECT_ORDER_BY_ID, [id])
                           ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return new Order(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ADRIAN_ID)),
                    dbDataReader.GetDateTime(dbDataReader.GetOrdinal(COL_DATE)),
                    dbDataReader.GetDouble(dbDataReader.GetOrdinal(COL_TOTAL_PRICE))
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read order with ID {OrderId} from database.", orderId);
            throw new NutriscanDatabaseException("Failed to read order from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for order with ID {OrderId}.", orderId);
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task<IReadOnlyCollection<Order>> ByUserId(Guid adrianId)
    {
        _logger.LogInformation("Fetching orders for user with AdrianId {AdrianId}.", adrianId);

        try
        {
            var orders = new List<Order>();

            using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException("Could not create DB connection.");
            connection.ConnectionString = _connectionString;
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = SELECT_ORDERS_BY_USER_ID;

            var param = command.CreateParameter();
            param.ParameterName = "@AdrianId";
            param.Value = adrianId.ToString().ToLower();
            command.Parameters.Add(param);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                orders.Add(new Order(
                    reader.GetGuid(reader.GetOrdinal(COL_ID)),
                    reader.GetGuid(reader.GetOrdinal(COL_ADRIAN_ID)),
                    reader.GetDateTime(reader.GetOrdinal(COL_DATE)),
                    reader.GetDouble(reader.GetOrdinal(COL_TOTAL_PRICE))
                ));
            }
            await connection.CloseAsync();
            return orders;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex,
                "Failed to fetch orders for user with AdrianId {AdrianId}.", adrianId);

            throw new NutriscanDatabaseException(
                "Failed to retrieve orders by user id from database.", ex);
        }
    }

    public async Task Save(Order order)
    {
        _logger.LogInformation("Saving order with ID {OrderId} to database.", order.OrderId);
        string orderQuery = INSERT_ORDER;

        if ((await ById(order.OrderId)) != null)
        {
            orderQuery = UPDATE_ORDER;
        }

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", order.OrderId.ToString().ToLower()),
                CreateParameter("@AdrianId", order.AdrianId.ToString().ToLower()),
                CreateParameter("@Date", order.Date),
                CreateParameter("@TotalPrice", order.TotalPrice)
            ];

            await ExecuteNonQueryAsync(orderQuery, parameters);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save order with ID {OrderId} to database.", order.OrderId);
            throw new NutriscanDatabaseException("Failed to save order to database.", ex);
        }
    }

    public async Task Remove(Order order)
    {
        _logger.LogInformation("Removing order with ID {OrderId} from database.", order.OrderId);

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", order.OrderId.ToString().ToLower())
            ];

            await ExecuteNonQueryAsync(DELETE_ORDER, parameters);
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to remove order with ID {OrderId} from database.", order.OrderId);
            throw new NutriscanDatabaseException("Failed to remove order from database.", ex);
        }
    }
}
