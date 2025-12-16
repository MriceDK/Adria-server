using System.Data.Common;
using Adria.Domain.Order;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoOrderSupplementRepository : AbstractAdoRepository, IOrderSupplementDetailsRepository
{
    private const string OrderId = "@OrderId";
    private const string SupplementId = "@SupplementId";
    
    private readonly ILogger<AdoOrderRepository> _logger;
    private static readonly string TABLE_ORDER_SUPPLEMENTS = "orderSupplementDetails";

    private static readonly string COL_ORDER_ID = "OrderId";
    private static readonly string COL_SUPPLEMENT_ID = "SupplementId";
    private static readonly string COL_AMOUNT = "Amount";

    private static readonly string INSERT_ORDER_SUPPLEMENT = $@"
    INSERT INTO {TABLE_ORDER_SUPPLEMENTS} ({COL_ORDER_ID}, {COL_SUPPLEMENT_ID}, {COL_AMOUNT})
    VALUES (@OrderId, @SupplementId, @Amount);
";

    private static readonly string UPDATE_ORDER_SUPPLEMENT = $@"
    UPDATE {TABLE_ORDER_SUPPLEMENTS}
    SET {COL_AMOUNT} = @Amount
    WHERE {COL_ORDER_ID} = @OrderId AND {COL_SUPPLEMENT_ID} = @SupplementId;
";

    private static readonly string SELECT_ORDER_SUPPLEMENT_BY_ORDER_AND_SUPPLEMENT = $@"
    SELECT {COL_ORDER_ID}, {COL_SUPPLEMENT_ID}, {COL_AMOUNT}
    FROM {TABLE_ORDER_SUPPLEMENTS}
    WHERE {COL_ORDER_ID} = @OrderId AND {COL_SUPPLEMENT_ID} = @SupplementId;
";

    private static readonly string SELECT_ORDER_SUPPLEMENTS_BY_ORDER = $@"
    SELECT {COL_ORDER_ID}, {COL_SUPPLEMENT_ID}, {COL_AMOUNT}
    FROM {TABLE_ORDER_SUPPLEMENTS}
    WHERE {COL_ORDER_ID} = @OrderId;
";
    
    private static readonly string SELECT_ORDER_SUPPLEMENTS_BY_SUPPLEMENT = $@"
    SELECT {COL_ORDER_ID}, {COL_SUPPLEMENT_ID}, {COL_AMOUNT}
    FROM {TABLE_ORDER_SUPPLEMENTS}
    WHERE {COL_SUPPLEMENT_ID} = @SupplementId;
";

    private static readonly string DELETE_ORDER_SUPPLEMENT = $@"
    DELETE FROM {TABLE_ORDER_SUPPLEMENTS}
    WHERE {COL_ORDER_ID} = @OrderId AND {COL_SUPPLEMENT_ID} = @SupplementId;
";
    

    public AdoOrderSupplementRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoOrderRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }
    
    public async Task<IReadOnlyCollection<OrderSupplementDetails>> ByOrderId(Guid orderId)
    {
        DbParameter param = CreateParameter(OrderId, orderId.ToString().ToLower());
        var dbDataReader = await ExecuteReaderAsync(SELECT_ORDER_SUPPLEMENTS_BY_ORDER, [param])
                           ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        var orderSupplements = new List<OrderSupplementDetails>();

        try
        {
            while (await dbDataReader.ReadAsync())
            {
                orderSupplements.Add(new OrderSupplementDetails(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ORDER_ID)),
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_SUPPLEMENT_ID)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_AMOUNT))
                ));
            }

            return orderSupplements;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read supplements for order with ID {OrderId} from database.", orderId);
            throw new NutriscanDatabaseException("Failed to read order supplements from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for order supplements with ID {OrderId}.", orderId);
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task<IReadOnlyCollection<OrderSupplementDetails>> BySupplementId(Guid supplementId)
    {
        DbParameter param = CreateParameter(SupplementId, supplementId.ToString().ToLower());
        var dbDataReader = await ExecuteReaderAsync(SELECT_ORDER_SUPPLEMENTS_BY_SUPPLEMENT, [param])
                           ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        var orderSupplements = new List<OrderSupplementDetails>();

        try
        {
            while (await dbDataReader.ReadAsync())
            {
                orderSupplements.Add(new OrderSupplementDetails(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ORDER_ID)),
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_SUPPLEMENT_ID)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_AMOUNT))
                ));
            }

            return orderSupplements;
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Failed to read orders for supplement with ID {SupplementId} from database.",
                supplementId
            );
            throw new NutriscanDatabaseException("Failed to read order supplements from database.", ex);
        }
        finally
        {
            _logger.LogInformation(
                "Disposing DbDataReader for orders with supplement ID {SupplementId}.",
                supplementId
            );
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task<OrderSupplementDetails> ByOrderAndSupplementId(Guid orderId, Guid supplementId)
    {
        DbParameter[] parameters =
        [
            CreateParameter(OrderId, orderId.ToString().ToLower()),
            CreateParameter(SupplementId, supplementId.ToString().ToLower())
        ];

        var dbDataReader = await ExecuteReaderAsync(
            SELECT_ORDER_SUPPLEMENT_BY_ORDER_AND_SUPPLEMENT,
            parameters
        ) ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        try
        {
            if (!await dbDataReader.ReadAsync())
            {
                throw new InvalidOperationException(
                    $"OrderSupplement not found for OrderId {orderId} and SupplementId {supplementId}."
                );
            }

            return new OrderSupplementDetails(
                dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ORDER_ID)),
                dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_SUPPLEMENT_ID)),
                dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_AMOUNT))
            );
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Failed to read OrderSupplement with OrderId {OrderId} and SupplementId {SupplementId} from database.",
                orderId,
                supplementId
            );
            throw new NutriscanDatabaseException(
                "Failed to read order supplement from database.",
                ex
            );
        }
        finally
        {
            _logger.LogInformation(
                "Disposing DbDataReader for OrderSupplement with OrderId {OrderId} and SupplementId {SupplementId}.",
                orderId,
                supplementId
            );
            await dbDataReader.DisposeAsync();
        }
    }


    
    public async Task Save(OrderSupplementDetails orderSupplementDetails)
    {
        _logger.LogInformation(
            "Saving OrderSupplement: OrderId {OrderId}, SupplementId {SupplementId} to database.",
            orderSupplementDetails.OrderId,
            orderSupplementDetails.SupplementId
        );

        // Check if the row exists
        var existing = await ByOrderAndSupplementId(orderSupplementDetails.OrderId, orderSupplementDetails.SupplementId);
        string query = existing == null ? INSERT_ORDER_SUPPLEMENT : UPDATE_ORDER_SUPPLEMENT;

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter(OrderId, orderSupplementDetails.OrderId.ToString().ToLower()),
                CreateParameter(SupplementId, orderSupplementDetails.SupplementId.ToString().ToLower()),
                CreateParameter("@Amount", orderSupplementDetails.Amount)
            ];

            await ExecuteNonQueryAsync(query, parameters);
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Failed to save OrderSupplement: OrderId {OrderId}, SupplementId {SupplementId}.",
                orderSupplementDetails.OrderId,
                orderSupplementDetails.SupplementId
            );
            throw new NutriscanDatabaseException("Failed to save OrderSupplement to database.", ex);
        }
    }

    public async Task Remove(OrderSupplementDetails orderSupplementDetails)
    {
        _logger.LogInformation(
            "Removing OrderSupplement: OrderId {OrderId}, SupplementId {SupplementId} from database.",
            orderSupplementDetails.OrderId,
            orderSupplementDetails.SupplementId
        );

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter(OrderId, orderSupplementDetails.OrderId.ToString().ToLower()),
                CreateParameter(SupplementId, orderSupplementDetails.SupplementId.ToString().ToLower())
            ];

            await ExecuteNonQueryAsync(DELETE_ORDER_SUPPLEMENT, parameters);
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Failed to remove OrderSupplement: OrderId {OrderId}, SupplementId {SupplementId}.",
                orderSupplementDetails.OrderId,
                orderSupplementDetails.SupplementId
            );
            throw new NutriscanDatabaseException("Failed to remove OrderSupplement from database.", ex);
        }
    }

}