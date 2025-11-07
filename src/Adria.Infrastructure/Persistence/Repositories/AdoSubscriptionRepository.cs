using System.Data.Common;
using Adria.Domain.Subcriptions;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoSubscriptionRepository: AbstractAdoRepository, ISubscriptionRepository
{
  private readonly ILogger<AdoSubscriptionRepository> _logger;
    private static readonly string TABLE_SUBSCRIPTIONS = "subscriptions";
    private static readonly string COL_ID = "id";
    private static readonly string COL_SUBSCRIPTION_TYPE = "type";
    private static readonly string COL_PRICE_PER_MONTH = "pricePerMonth";
    private static readonly string COL_ADVANTAGES = "advantages";
    
    private static readonly string INSERT_SUBSCRIPTION = $@"
        INSERT INTO {TABLE_SUBSCRIPTIONS} ({COL_ID}, {COL_SUBSCRIPTION_TYPE}, {COL_PRICE_PER_MONTH}, {COL_ADVANTAGES})
        VALUES (@Id, @SubscriptionType, @PricePerMonth, @Advantages);
    ";

    private static readonly string UPDATE_SUBSCRIPTION = $@"
        UPDATE {TABLE_SUBSCRIPTIONS}
        SET {COL_SUBSCRIPTION_TYPE} = @SubscriptionType, 
            {COL_PRICE_PER_MONTH} = @PricePerMonth, 
            {COL_ADVANTAGES} = @Advantages
        WHERE {COL_ID} = @Id;
    ";

    private static readonly string SELECT_SUBSCRIPTION_BY_ID = $@"
        SELECT {COL_ID}, {COL_SUBSCRIPTION_TYPE}, {COL_PRICE_PER_MONTH}, {COL_ADVANTAGES}
        FROM {TABLE_SUBSCRIPTIONS}
        WHERE {COL_ID} = @Id;
    ";

    public AdoSubscriptionRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoSubscriptionRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<Subscription?> ById(Guid subscriptionId)
    {
        DbParameter id = CreateParameter("@Id", subscriptionId.ToString().ToLower());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_SUBSCRIPTION_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
       
                return new Subscription(
                   
                    (SubscriptionType)Enum.Parse(typeof(SubscriptionType), dbDataReader.GetString(dbDataReader.GetOrdinal(COL_SUBSCRIPTION_TYPE))),
                    dbDataReader.GetDouble(dbDataReader.GetOrdinal(COL_PRICE_PER_MONTH)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_ADVANTAGES)),
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID))
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read subscription with ID {SubscriptionId} from database.", subscriptionId);
            throw new NutriscanDatabaseException("Failed to read subscription from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for subscription with ID {SubscriptionId}.", subscriptionId);
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task Save(Subscription subscription)
    {
        _logger.LogInformation("Saving subscription with ID {SubscriptionId} to database.", subscription.Id);
        string subscriptionQuery = INSERT_SUBSCRIPTION;

        if ((await ById(subscription.Id)) != null)
        {
            subscriptionQuery = UPDATE_SUBSCRIPTION;
        }

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", subscription.Id.ToString().ToLower()),
                CreateParameter("@SubscriptionType", subscription.SubscriptionType.ToString()),
                CreateParameter("@PricePerMonth", subscription.PricePerMonth),
                CreateParameter("@Advantages", subscription.Advantages)
            ];

            await ExecuteNonQueryAsync(subscriptionQuery, parameters);

        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save subscription with ID {SubscriptionId} to database.", subscription.Id);
            throw new NutriscanDatabaseException("Failed to save subscription to database.", ex);
        }
    }
}