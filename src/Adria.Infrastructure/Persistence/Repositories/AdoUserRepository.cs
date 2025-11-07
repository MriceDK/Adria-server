using System.Data.Common;
using Adria.Domain.Subcriptions;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoUserRepository : AbstractAdoRepository, IUserRepository
{
    private readonly ILogger<AdoUserRepository> _logger;
    private readonly ISubscriptionRepository _subscriptionRepository;

    private static readonly string TABLE_USERS = "users";
    private static readonly string COL_ADRIAN_ID = "AdrianId";
    private static readonly string COL_NAME = "Name";
    private static readonly string COL_JOB = "Job";
    private static readonly string COL_SUBSCRIPTION_ID = "SubscriptionId";
    
    private static readonly string INSERT_USER = $@"
        INSERT INTO {TABLE_USERS} ({COL_ADRIAN_ID}, {COL_NAME}, {COL_JOB}, {COL_SUBSCRIPTION_ID})
        VALUES (@Id, @Name, @Job, @SubscriptionId);
    ";

    private static readonly string UPDATE_USER = $@"
        UPDATE {TABLE_USERS}
        SET {COL_NAME} = @Name, 
            {COL_JOB} = @Job, 
            {COL_SUBSCRIPTION_ID} = @SubscriptionId
        WHERE {COL_ADRIAN_ID} = @Id;
    ";

    private static readonly string SELECT_USER_BY_ID = $@"
        SELECT {COL_ADRIAN_ID}, {COL_NAME}, {COL_JOB}, {COL_SUBSCRIPTION_ID}
        FROM {TABLE_USERS}
        WHERE {COL_ADRIAN_ID} = @Id;
    ";

    public AdoUserRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoUserRepository> logger,
        ISubscriptionRepository subscriptionRepository 
    ) : base(factory, connectionString)
    {
        _logger = logger;
        _subscriptionRepository = subscriptionRepository; 
    }

    public async Task<User?> ById(Guid userId)
    {
        DbParameter id = CreateParameter("@Id", userId.ToString().ToLower());
        DbDataReader dbDataReader = await ExecuteReaderAsync(SELECT_USER_BY_ID, [id]);

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                var adrianId = Guid.Parse(dbDataReader.GetString(dbDataReader.GetOrdinal(COL_ADRIAN_ID)));
                var name = dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME));
                var job = dbDataReader.GetString(dbDataReader.GetOrdinal(COL_JOB));
                var subscriptionId = Guid.Parse(dbDataReader.GetString(dbDataReader.GetOrdinal(COL_SUBSCRIPTION_ID)));

                Subscription? subscription = await _subscriptionRepository.ById(subscriptionId);
                if (subscription is null)
                {
                    _logger.LogError("Data integrity error: User {UserId} references non-existent Subscription {SubscriptionId}", userId, subscriptionId);
                    throw new NutriscanDatabaseException($"User {userId} references non-existent Subscription {subscriptionId}.", null);
                }

                return new User(
                    name,
                    job,
                    subscription, 
                    adrianId
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read user with ID {UserId} from database.", userId);
            throw new NutriscanDatabaseException("Failed to read user from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for user with ID {UserId}.", userId);
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task Save(User user)
    {
        _logger.LogInformation("Saving user with ID {UserId} to database.", user.AdriaId);
        string userQuery = INSERT_USER;

        if ((await ById(user.AdriaId)) != null)
        {
            userQuery = UPDATE_USER;
        }

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", user.AdriaId.ToString().ToLower()),
                CreateParameter("@Name", user.Name),
                CreateParameter("@Job", user.Job),
                // Domain nesnesinden sadece ID'yi alıp veritabanına yaz
                CreateParameter("@SubscriptionId", user.Subscription.Id.ToString().ToLower())
            ];

            await ExecuteNonQueryAsync(userQuery, parameters);

        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save user with ID {UserId} to database.", user.AdriaId);
            throw new NutriscanDatabaseException("Failed to save user to database.", ex);
        }
    }
}