using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class LatestBodyStatsQuery : IBodyStatsQuery
{
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<LatestBodyStatsQuery> _logger;

    private static readonly string QRY = @"
        SELECT 
            bs.BodyStatId,
            bs.Label,
            bs.Unit,
            bs.Goal,
            ha.Current
        FROM healthAnalyse ha
        JOIN bodyStats bs ON ha.BodyStatId = bs.BodyStatId
        WHERE ha.AnalyseId = (
            SELECT AnalyseId 
            FROM analyse 
            WHERE AdrianId = @UserId 
            ORDER BY DateTime DESC 
            LIMIT 1
        );
    ";

    public LatestBodyStatsQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<LatestBodyStatsQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<BodyStatData>?> Fetch(Guid userId)
    {
        _logger.LogInformation("Fetching latest body stats for User {UserId}", userId);

        using var connection = _factory.CreateConnection()
             ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@UserId";
        parameter.Value = userId.ToString().ToLower(); 
        command.Parameters.Add(parameter);

        using var reader = await command.ExecuteReaderAsync();
        var result = new List<BodyStatData>();

        while (await reader.ReadAsync())
        {   
            var bodyStatIdOrd = reader.GetOrdinal("BodyStatId");
            var labelOrd = reader.GetOrdinal("Label");
            var unitOrd = reader.GetOrdinal("Unit");
            var goalOrd = reader.GetOrdinal("Goal");
            var currentOrd = reader.GetOrdinal("Current");

            var label = reader.GetString(labelOrd);
            var bodyStatId = reader.GetString(bodyStatIdOrd);
            var unit = await reader.IsDBNullAsync(unitOrd) 
                ? null 
                : reader.GetString(unitOrd);
                
            var goal = await reader.IsDBNullAsync(goalOrd) 
                ? (double?)null 
                : reader.GetDouble(goalOrd);
                
            var val = reader.GetDouble(currentOrd);

            string formattedCurrent = (unit == "%") ? $"{val}%" : val.ToString();

            result.Add(new BodyStatData(bodyStatId,label, formattedCurrent, goal, unit));
        }

        return result.AsReadOnly();
    }
}