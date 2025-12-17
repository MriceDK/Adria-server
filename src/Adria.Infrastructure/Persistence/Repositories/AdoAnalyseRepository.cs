using System.Data.Common;
using Adria.Domain.BodyStats;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoAnalyseRepository : AbstractAdoRepository, IAnalyseRepository 
{
    private readonly ILogger<AdoAnalyseRepository> _logger;

    private static readonly string TABLE_ANALYSE = "analyse";
    private static readonly string TABLE_HEALTH_ANALYSE = "healthAnalyse";
    private static readonly string COL_ANALYSE_ID = "AnalyseId";
    private static readonly string COL_ADRIAN_ID = "AdrianId";
    private static readonly string COL_DATE_TIME = "DateTime";
    private static readonly string COL_BODY_STAT_ID = "BodyStatId";
    private static readonly string COL_CURRENT = "Current";

    private static readonly string INSERT_ANALYSE = $@"
        INSERT INTO {TABLE_ANALYSE} ({COL_ANALYSE_ID}, {COL_ADRIAN_ID}, {COL_DATE_TIME})
        VALUES (@AnalyseId, @AdrianId, @DateTime);
    ";

    private static readonly string INSERT_HEALTH_ANALYSE = $@"
        INSERT INTO {TABLE_HEALTH_ANALYSE} ({COL_ANALYSE_ID}, {COL_BODY_STAT_ID}, {COL_CURRENT})
        VALUES (@AnalyseId, @BodyStatId, @Current);
    ";

    public AdoAnalyseRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoAnalyseRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task Save(Analyse analyse)
    {
        _logger.LogInformation("Starting transaction to save Analyse with ID {AnalyseId}.", analyse.Id);
        
        using var connection = _factory.CreateConnection() 
             ?? throw new InvalidOperationException("Failed to create database connection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var analyseParams = new[]
            {
                CreateParameter("@AnalyseId", analyse.Id),
                CreateParameter("@AdrianId", analyse.AdrianId.ToString().ToLower()),
                CreateParameter("@DateTime", analyse.DateTime)
            };

            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = INSERT_ANALYSE;
                command.Parameters.AddRange(analyseParams);
                await command.ExecuteNonQueryAsync();
            }

            foreach (var detail in analyse.Details)
            {
                var detailParams = new[]
                {
                    CreateParameter("@AnalyseId", analyse.Id.ToString().ToLower()),
                    CreateParameter("@BodyStatId", detail.BodyStatId.ToString().ToLower()),
                    CreateParameter("@Current", detail.CurrentValue)
                };

                using (var command = connection.CreateCommand())
                {
                    command.Transaction = transaction;
                    command.CommandText = INSERT_HEALTH_ANALYSE;
                    command.Parameters.AddRange(detailParams);
                    await command.ExecuteNonQueryAsync();
                }
            }

            await transaction.CommitAsync();
            _logger.LogInformation("Analyse {AnalyseId} saved successfully.", analyse.Id);
            await connection.CloseAsync();
        }
        catch (DbException ex)
        {
            await transaction.RollbackAsync();
            await connection.CloseAsync();
            _logger.LogError(ex, "Failed to save Analyse. Transaction rolled back.");
            throw new NutriscanDatabaseException("Failed to save analyse to database.", ex);
        }
    }
}
