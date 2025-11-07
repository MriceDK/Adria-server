using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class UserByIdQuery : IUserByIdQuery
{
   private static readonly string QRY = @"
        SELECT u.AdrianId, u.Name, u.Job, s.Type AS SubscriptionType
        FROM users u
        INNER JOIN subscriptions s ON u.SubscriptionId = s.id
        WHERE u.AdrianId = @Id;
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<UserByIdQuery> _logger;

    public UserByIdQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<UserByIdQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<UserData?> Fetch(Guid userId)
    {
        _logger.LogInformation("Starting to fetch user with ID {UserId}", userId);

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = QRY;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "@Id";
        parameter.Value = userId.ToString().ToLower();
        command.Parameters.Add(parameter);

        using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var idOrd = reader.GetOrdinal("AdrianId");
            var nameOrd = reader.GetOrdinal("Name");
            var jobOrd = reader.GetOrdinal("Job");
            var subTypeOrd = reader.GetOrdinal("SubscriptionType"); 

            var id = Guid.Parse(reader.GetString(idOrd)); 
            var name = reader.GetString(nameOrd);
            var job = reader.GetString(jobOrd);
            var subType = reader.GetString(subTypeOrd);

            _logger.LogInformation("Found user with ID {UserId}", userId);
            
            return new UserData(id, name, job, subType);
        }

        _logger.LogInformation("User with ID {UserId} not found", userId);
        return null;
    }
}