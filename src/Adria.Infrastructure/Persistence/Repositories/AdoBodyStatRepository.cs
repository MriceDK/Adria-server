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


    public async Task<BodyStat?> ById(string id)
    {
        var param = CreateParameter("@Id", id);
        
        var reader = await ExecuteReaderAsync(SELECT_BY_ID, [param]);

        try
        {
            if (await reader.ReadAsync())
            {
                var labelOrd = reader.GetOrdinal(COL_LABEL);
                var unitOrd = reader.GetOrdinal(COL_UNIT);
                var goalOrd = reader.GetOrdinal(COL_GOAL);
                var idOrd = reader.GetOrdinal(COL_ID);

                var label = reader.GetString(labelOrd);
                
                var unit = await reader.IsDBNullAsync(unitOrd) 
                    ? null 
                    : reader.GetString(unitOrd);
                    
                var goal = await reader.IsDBNullAsync(goalOrd) 
                    ? (double?)null 
                    : reader.GetDouble(goalOrd);
                
                var statId = reader.GetString(idOrd);

                return new BodyStat(label, unit, goal, statId);
            }

            return null;
        }
        finally
        {
            await reader.DisposeAsync();
        }
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