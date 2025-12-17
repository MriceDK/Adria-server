using System.Data;
using System.Data.Common;
using Adria.Application.PushNotifications;
using Adria.Domain.Scanner;
using Adria.Infrastructure.Persistence.Shared;
using Adria.Infrastructure.PushNotifications;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoScanFood : AbstractAdoRepository, IScan
{
    private readonly ILogger<AdoScanFood> _logger;
    private readonly INotificationSender _notificationSender;


    private const string TABLE = "scans";
    private const string COL_SCAN_ID = "ScanId";
    private const string COL_ADRIAN_ID = "AdrianId";
    private const string COL_DATETIME = "DateTime";
    private const string COL_RESULT = "Result";
    private const string COL_FOOD_ID = "FoodId";

    private readonly string INSERT_QUERY = $@"
        INSERT INTO {TABLE} 
        ({COL_SCAN_ID}, {COL_ADRIAN_ID}, {COL_DATETIME}, {COL_RESULT}, {COL_FOOD_ID})
        VALUES (@ScanId, @AdrianId, @DateTime, @Result, @FoodId);
    ";

    private readonly string SELECT_BY_ID = $@"
        SELECT {COL_SCAN_ID}, {COL_ADRIAN_ID}, {COL_DATETIME}, {COL_RESULT}, {COL_FOOD_ID}
        FROM {TABLE}
        WHERE {COL_SCAN_ID} = @ScanId;
    ";

    private readonly string SELECT_BY_USER = $@"
        SELECT {COL_SCAN_ID}, {COL_ADRIAN_ID}, {COL_DATETIME}, {COL_RESULT}, {COL_FOOD_ID}
        FROM {TABLE}
        WHERE {COL_ADRIAN_ID} = @AdrianId
        ORDER BY {COL_DATETIME} DESC;
    ";

    private readonly string DELETE_QUERY = $@"
        DELETE FROM {TABLE}
        WHERE {COL_SCAN_ID} = @ScanId;
    ";

    public AdoScanFood(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoScanFood> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task Save(Scan scan)
    {
        try
        {
            await ExecuteNonQueryAsync(
                INSERT_QUERY,
                new[]
                {
                    CreateParameter("@ScanId", scan.ScanId),
                    CreateParameter("@AdrianId", scan.AdrianId),
                    CreateParameter("@DateTime", scan.DateTime),
                    CreateParameter("@Result", scan.Result),
                    CreateParameter("@FoodId", scan.FoodId)
                }
            );

            await _notificationSender.Send(
                "New item scanned",
                $"{scan.Result}\nTime: {scan.DateTime:dd.MM.yyyy HH:mm}"
            );
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to save scan {ScanId}", scan.ScanId);
            throw new InvalidOperationException(
                $"Failed to persist scan with ScanId {scan.ScanId}.",
                ex
            );
        }
    }

    public async Task Remove(Guid scanId)
    {
        try
        {
            await ExecuteNonQueryAsync(
                DELETE_QUERY,
                new[]
                {
                    CreateParameter("@ScanId", scanId)
                }
            );
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to remove scan {ScanId}", scanId);
            throw new InvalidOperationException(
                $"Failed to remove scan with ScanId {scanId}.",
                ex
            );
        }
    }


    public async Task<Scan?> ById(Guid scanId)
    {
        using var reader = await ExecuteReaderAsync(
            SELECT_BY_ID,
            new[] { CreateParameter("@ScanId", scanId) }
        );

        if (await reader.ReadAsync())
        {
            return new Scan(
                reader.GetGuid(reader.GetOrdinal(COL_SCAN_ID)),
                reader.GetGuid(reader.GetOrdinal(COL_ADRIAN_ID)),
                reader.GetDateTime(reader.GetOrdinal(COL_DATETIME)),
                reader.GetString(reader.GetOrdinal(COL_RESULT)),
                reader.GetString(reader.GetOrdinal(COL_FOOD_ID))
            );
        }

        return null;
    }

    public async Task<IReadOnlyCollection<Scan>> ByUserId(Guid adrianId)
    {
        var scans = new List<Scan>();

        using var reader = await ExecuteReaderAsync(
            SELECT_BY_USER,
            new[] { CreateParameter("@AdrianId", adrianId) }
        );

        while (await reader.ReadAsync())
        {
            scans.Add(new Scan(
                reader.GetGuid(reader.GetOrdinal(COL_SCAN_ID)),
                reader.GetGuid(reader.GetOrdinal(COL_ADRIAN_ID)),
                reader.GetDateTime(reader.GetOrdinal(COL_DATETIME)),
                reader.GetString(reader.GetOrdinal(COL_RESULT)),
                reader.GetString(reader.GetOrdinal(COL_FOOD_ID))
            ));
        }

        return scans;
    }
}