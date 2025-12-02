using System.Data.Common;
using Adria.Domain.BodyStats;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;
using IBodyStatRepository = Adria.Application.Contracts.IBodyStatRepository;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoBodyStatRepository(
    DbProviderFactory factory,
    string connectionString,
    ILogger<AdoBodyStatRepository> logger)
    : AbstractAdoRepository(factory, connectionString), IBodyStatRepository
{
    private static readonly string TABLE = "bodyStats";
    private static readonly string COL_ID = "BodyStatId";
    private static readonly string COL_LABEL = "Label";
    private static readonly string COL_UNIT = "Unit";
    private static readonly string COL_GOAL = "Goal";

    private static readonly string UPDATE_GOAL_SQL = $@"
        UPDATE {TABLE}
        SET {COL_GOAL} = @Goal
        WHERE {COL_ID} = @Id;
    ";

    private static readonly string SELECT_BY_ID = $@"
        SELECT {COL_ID}, {COL_LABEL}, {COL_UNIT}, {COL_GOAL}
        FROM {TABLE}
        WHERE {COL_ID} = @Id;
    ";

    public async Task<BodyStat?> ById(Guid id)
    {
        var param = CreateParameter("@Id", id.ToString().ToLower());
        var reader = await ExecuteReaderAsync(SELECT_BY_ID, [param]);

        if (await reader.ReadAsync())
        {
            var label = reader.GetString(reader.GetOrdinal(COL_LABEL));
            var unit = reader.IsDBNull(reader.GetOrdinal(COL_UNIT)) ? null : reader.GetString(reader.GetOrdinal(COL_UNIT));
            var goal = reader.IsDBNull(reader.GetOrdinal(COL_GOAL)) ? (double?)null : reader.GetDouble(reader.GetOrdinal(COL_GOAL));
            var statId = Guid.Parse(reader.GetString(reader.GetOrdinal(COL_ID)));

            await reader.DisposeAsync();
            return new BodyStat(label, unit, goal, statId);
        }
        
        await reader.DisposeAsync();
        return null;
    }

    public async Task Update(BodyStat bodyStat)
    {
        try
        {
            var parameters = new[]
            {
                CreateParameter("@Id", bodyStat.Id.ToString().ToLower()),
                CreateParameter("@Goal", bodyStat.Goal ?? (object)DBNull.Value) 
            };

            await ExecuteNonQueryAsync(UPDATE_GOAL_SQL, parameters);
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to update BodyStat {Id}", bodyStat.Id);
            throw new NutriscanDatabaseException("Failed to update BodyStat", ex);
        }
    }
}