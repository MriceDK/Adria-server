using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class AllUsersQuery : IAllUsersQuery
{
    private static readonly string QRY = @"
        SELECT u.AdrianId, u.Name, u.Job, s.Type AS SubscriptionType
        FROM users u
        INNER JOIN subscriptions s ON u.SubscriptionId = s.id
    ";

    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<AllUsersQuery> _logger;

    public AllUsersQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AllUsersQuery> logger) // Logger tipini düzelttim
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<UserData>> Fetch()
    {
        _logger.LogInformation("Starting to fetch all users");

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException(
                                   "DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        using var reader = await command.ExecuteReaderAsync();

        var users = new List<UserData>();

        while (await reader.ReadAsync())
        {
            // --- SÜTUN ADLARINIZA GÖRE GÜNCELLENDİ ---
            var idOrd = reader.GetOrdinal("AdrianId");
            var nameOrd = reader.GetOrdinal("Name");
            var jobOrd = reader.GetOrdinal("Job");
            var subTypeOrd = reader.GetOrdinal("SubscriptionType");

            var id = Guid.Parse(reader.GetString(idOrd)); 
            var name = reader.GetString(nameOrd);
            var job = reader.GetString(jobOrd);
            var subType = reader.GetString(subTypeOrd);

            users.Add(new UserData(
                id,
                name,
                job,
                subType
            ));
        }

        _logger.LogInformation("Fetched {Count} users", users.Count);
        await connection.CloseAsync();
        return users.AsReadOnly();
    }
}