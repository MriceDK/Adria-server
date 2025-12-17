using System.Data;
using System.Data.Common;

namespace Adria.Infrastructure.Persistence.Shared;

public class AbstractAdoRepository
{
    
    protected readonly DbProviderFactory _factory;
    protected readonly string _connectionString;

    protected AbstractAdoRepository(
        DbProviderFactory factory,
        string connectionString
    )
    {
        _factory = factory;
        _connectionString = connectionString;
    }

    protected DbParameter CreateParameter(string name, object value)
    {
        var parameter = _factory.CreateParameter()!;
        parameter.ParameterName = name;
        parameter.Value = value;
        return parameter;
    }

    protected async Task ExecuteNonQueryAsync(string commandText, DbParameter[] parameters)
    {
        try
        {
            await using var connection = await OpenConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.AddRange(parameters);
            await command.ExecuteNonQueryAsync();
        }
        catch (DbException ex)
        {
            throw new NutriscanDatabaseException("Database operation failed.", ex);
        }
    }

    protected async Task<DbDataReader> ExecuteReaderAsync(
        string commandText,
        DbParameter[] parameters
    )
    {
        var connection = await OpenConnection();
        var command = connection.CreateCommand();
        command.CommandText = commandText;
        command.Parameters.AddRange(parameters);

        return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
    }

    protected async Task<DbConnection> OpenConnection()
    {
        var connection = _factory.CreateConnection() ??
             throw new InvalidOperationException("Failed to create a database connection.");

        connection.ConnectionString = _connectionString;

        await connection.OpenAsync();

        return connection;
    }
    
    protected async Task ExecuteInTransactionAsync(Func<DbConnection, DbTransaction, Task> action)
    {
        await using var connection = _factory.CreateConnection()
                                     ?? throw new InvalidOperationException("Failed to create a database connection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await action(connection, transaction);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        await connection.CloseAsync();
    }
}