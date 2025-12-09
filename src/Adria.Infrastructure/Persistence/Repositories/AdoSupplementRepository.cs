using System.Data.Common;
using Adria.Domain.Order;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoSupplementRepository :  AbstractAdoRepository, ISupplementRepository
{
    private readonly ILogger<AdoSupplementRepository> _logger;

    private static readonly string TABLE_SUPPLEMENTS = "supplements";

    private static readonly string COL_ID = "id";
    private static readonly string COL_NAME = "name";
    private static readonly string COL_TYPE = "type";
    private static readonly string COL_PRICE = "price";
    private static readonly string COL_STOCK = "stock";

    private static readonly string INSERT_SUPPLEMENT = $@"
    INSERT INTO {TABLE_SUPPLEMENTS} ({COL_ID}, {COL_NAME}, {COL_TYPE}, {COL_PRICE}, {COL_STOCK})
    VALUES (@Id, @Name, @Type, @Price, @Stock);
";

    private static readonly string UPDATE_SUPPLEMENT = $@"
    UPDATE {TABLE_SUPPLEMENTS}
    SET {COL_NAME} = @Name,
        {COL_TYPE} = @Type,
        {COL_PRICE} = @Price,
        {COL_STOCK} = @Stock
    WHERE {COL_ID} = @Id;
";

    private static readonly string SELECT_SUPPLEMENT_BY_ID = $@"
    SELECT {COL_ID}, {COL_NAME}, {COL_TYPE}, {COL_PRICE}, {COL_STOCK}
    FROM {TABLE_SUPPLEMENTS}
    WHERE {COL_ID} = @Id;
";

    private static readonly string SELECT_SUPPLEMENTS_BY_NAME = $@"
    SELECT {COL_ID}, {COL_NAME}, {COL_TYPE}, {COL_PRICE}, {COL_STOCK}
    FROM {TABLE_SUPPLEMENTS}
    WHERE {COL_NAME} = @Name;
";

    private static readonly string SELECT_SUPPLEMENTS_BY_TYPE = $@"
    SELECT {COL_ID}, {COL_NAME}, {COL_TYPE}, {COL_PRICE}, {COL_STOCK}
    FROM {TABLE_SUPPLEMENTS}
    WHERE {COL_TYPE} = @Type;
";

    private static readonly string SELECT_ALL_SUPPLEMENTS = $@"
    SELECT {COL_ID}, {COL_NAME}, {COL_TYPE}, {COL_PRICE}, {COL_STOCK}
    FROM {TABLE_SUPPLEMENTS};
";

    private static readonly string DELETE_SUPPLEMENT = $@"
    DELETE FROM {TABLE_SUPPLEMENTS}
    WHERE {COL_ID} = @Id;
";

    public AdoSupplementRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoSupplementRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }
    
    public async Task<IReadOnlyCollection<Supplement>> GetAll()
    {
        var dbDataReader = await ExecuteReaderAsync(SELECT_ALL_SUPPLEMENTS, Array.Empty<DbParameter>())
                           ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        var supplements = new List<Supplement>();

        try
        {
            while (await dbDataReader.ReadAsync())
            {
                supplements.Add(new Supplement(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_TYPE)),
                    dbDataReader.GetDouble(dbDataReader.GetOrdinal(COL_PRICE)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_STOCK))
                ));
            }

            return supplements;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read all supplements from database.");
            throw new NutriscanDatabaseException("Failed to read all supplements from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for GetAll supplements.");
            await dbDataReader.DisposeAsync();
        }
    }

    
    public async Task<Supplement?> ById(Guid supplementId)
    {
        DbParameter id = CreateParameter("@Id", supplementId.ToString().ToLower());
        var dbDataReader = await ExecuteReaderAsync(SELECT_SUPPLEMENT_BY_ID, [id])
                           ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        try
        {
            if (await dbDataReader.ReadAsync())
            {
                return new Supplement(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_TYPE)),
                    dbDataReader.GetDouble(dbDataReader.GetOrdinal(COL_PRICE)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_STOCK))
                );
            }

            return null;
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Failed to read supplement with ID {SupplementId} from database.",
                supplementId
            );

            throw new NutriscanDatabaseException("Failed to read supplement from database.", ex);
        }
        finally
        {
            _logger.LogInformation(
                "Disposing DbDataReader for supplement with ID {SupplementId}.",
                supplementId
            );
            await dbDataReader.DisposeAsync();
        }
    }
    
    public async Task<IReadOnlyCollection<Supplement>> ByName(string name)
    {
        DbParameter param = CreateParameter("@Name", name);
        var dbDataReader = await ExecuteReaderAsync(SELECT_SUPPLEMENTS_BY_NAME, [param])
                           ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        var supplements = new List<Supplement>();

        try
        {
            while (await dbDataReader.ReadAsync())
            {
                supplements.Add(new Supplement(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_TYPE)),
                    dbDataReader.GetDouble(dbDataReader.GetOrdinal(COL_PRICE)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_STOCK))
                ));
            }

            return supplements;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read supplements with Name {Name} from database.", name);
            throw new NutriscanDatabaseException("Failed to read supplements by name from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for supplements with Name {Name}.", name);
            await dbDataReader.DisposeAsync();
        }
    }

    public async Task<IReadOnlyCollection<Supplement>> ByType(string type)
    {
        DbParameter param = CreateParameter("@Type", type);
        var dbDataReader = await ExecuteReaderAsync(SELECT_SUPPLEMENTS_BY_TYPE, [param])
                           ?? throw new InvalidOperationException("Failed to create DbDataReader.");

        var supplements = new List<Supplement>();

        try
        {
            while (await dbDataReader.ReadAsync())
            {
                supplements.Add(new Supplement(
                    dbDataReader.GetGuid(dbDataReader.GetOrdinal(COL_ID)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_NAME)),
                    dbDataReader.GetString(dbDataReader.GetOrdinal(COL_TYPE)),
                    dbDataReader.GetDouble(dbDataReader.GetOrdinal(COL_PRICE)),
                    dbDataReader.GetInt32(dbDataReader.GetOrdinal(COL_STOCK))
                ));
            }

            return supplements;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read supplements with Type {Type} from database.", type);
            throw new NutriscanDatabaseException("Failed to read supplements by type from database.", ex);
        }
        finally
        {
            _logger.LogInformation("Disposing DbDataReader for supplements with Type {Type}.", type);
            await dbDataReader.DisposeAsync();
        }
    }

    
    public async Task Save(Supplement supplement)
    {
        _logger.LogInformation("Saving supplement with ID {SupplementId} to database.", supplement.SupplementId);
        string supplementQuery = INSERT_SUPPLEMENT;

        if ((await ById(supplement.SupplementId)) != null)
        {
            supplementQuery = UPDATE_SUPPLEMENT;
        }

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", supplement.SupplementId.ToString().ToLower()),
                CreateParameter("@Name", supplement.Name),
                CreateParameter("@Type", supplement.Type),
                CreateParameter("@Price", supplement.Price),
                CreateParameter("@Stock", supplement.Stock)
            ];

            await ExecuteNonQueryAsync(supplementQuery, parameters);
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Failed to save supplement with ID {SupplementId} to database.",
                supplement.SupplementId
            );

            throw new NutriscanDatabaseException("Failed to save supplement to database.", ex);
        }
    }
    
    public async Task Remove(Supplement supplement)
    {
        _logger.LogInformation("Removing supplement with ID {SupplementId} from database.", supplement.SupplementId);

        try
        {
            DbParameter[] parameters =
            [
                CreateParameter("@Id", supplement.SupplementId.ToString().ToLower())
            ];

            await ExecuteNonQueryAsync(DELETE_SUPPLEMENT, parameters);
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Failed to remove supplement with ID {SupplementId} from database.",
                supplement.SupplementId
            );

            throw new NutriscanDatabaseException("Failed to remove supplement from database.", ex);
        }
    }


}