using System.Data.Common;
using Adria.Domain.Order;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoOrderRepository: AbstractAdoRepository, IOrderRepository
{
  private readonly ILogger<AdoOrderRepository> _logger;
    private static readonly string TABLE_ORDERS = "orders";
    private static readonly string COL_ID = "id";
    private static readonly string COL_ADRIAN_ID = "adrianId";
    private static readonly string COL_DATE = "date";
    private static readonly string COL_TOTAL_PRICE = "totalPrice";
    
    private static readonly string INSERT_ORDER = $@"
        INSERT INTO {TABLE_ORDERS} ({COL_ID}, {COL_ADRIAN_ID}, {COL_DATE}, {COL_TOTAL_PRICE})
        VALUES (@Id, @AdrianId, @Date, @TotalPrice);
    ";

    private static readonly string UPDATE_ORDER = $@"
        UPDATE {TABLE_ORDERS}
        SET {COL_ADRIAN_ID} = @SubscriptionType, 
            {COL_DATE} = @PricePerMonth, 
            {COL_TOTAL_PRICE} = @Advantages
        WHERE {COL_ID} = @Id;
    ";

    private static readonly string SELECT_ORDER_BY_ID = $@"
        SELECT {COL_ID}, {COL_ADRIAN_ID}, {COL_DATE}, {COL_TOTAL_PRICE}
        FROM {TABLE_ORDERS}
        WHERE {COL_ID} = @Id;
    ";

    public AdoOrderRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoOrderRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<Order?> ById(Guid orderId)
    {
        DbParameter id = CreateParameter("@Id", orderId.ToString().ToLower());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_ORDER_BY_ID, [id]);

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
}