using System.Data.Common;
using Adria.Domain.Food;
using Adria.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoNutrientRepository : AbstractAdoRepository, INutrient
{
    private const string CouldNotCreateDbConnectionMessage = "Could not create DB connection.";
    private const string NUTRIENT_ID = "nutrientId";

    public AdoNutrientRepository(DbProviderFactory factory, string connectionString)
        : base(factory, connectionString) { }

    public async Task<Nutrient?> ById(string nutrientId)
    {
        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException(CouldNotCreateDbConnectionMessage);
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT NutrientId, Type, Unit, Minimum, Maximum FROM nutrients WHERE NutrientId = @Id";

        var param = command.CreateParameter();
        param.ParameterName = "@Id";
        param.Value = nutrientId;
        command.Parameters.Add(param);
        
        
        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Nutrient(
            reader.GetString(reader.GetOrdinal(NUTRIENT_ID)),
            reader.GetString(reader.GetOrdinal("Type")),
            reader.GetString(reader.GetOrdinal("Unit"))
        );
    }

    public async Task<IReadOnlyCollection<Nutrient>> ByType(string type)
    {
        var nutrients = new List<Nutrient>();

        const string query = @"
            SELECT NutrientId, Type, RecommendedAmount
            FROM Nutrients
            WHERE Type = @Type;
        ";

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException(CouldNotCreateDbConnectionMessage);
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        var param = command.CreateParameter();
        param.ParameterName = "@Type";
        param.Value = type;
        command.Parameters.Add(param);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            nutrients.Add(new Nutrient(
                reader.GetString(reader.GetOrdinal(NUTRIENT_ID)),
                reader.GetString(reader.GetOrdinal("Type")),
                reader.GetString(reader.GetOrdinal("Unit"))
            ));
        }

        return nutrients;
    }

    public async Task Remove(Nutrient nutrient)
    {
        const string query = @"
            DELETE FROM Nutrients
            WHERE NutrientId = @NutrientId;
        ";

        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException(CouldNotCreateDbConnectionMessage);
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        var param = command.CreateParameter();
        param.ParameterName = "@NutrientId";
        param.Value = nutrient.NutrientId;
        command.Parameters.Add(param);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<IReadOnlyCollection<Nutrient>> GetAllNutrients()
    {
        var nutrients = new List<Nutrient>();

        using var connection = _factory.CreateConnection() 
            ?? throw new InvalidOperationException(CouldNotCreateDbConnectionMessage);
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT NutrientId, Type, RecommendedAmount FROM Nutrients";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            nutrients.Add(new Nutrient(
                reader.GetString(reader.GetOrdinal(NUTRIENT_ID)),
                reader.GetString(reader.GetOrdinal("Type")),
                reader.GetString(reader.GetOrdinal("Unit"))
            ));
        }

        return nutrients;
    }

    public async Task<IReadOnlyCollection<string>> GetNutrientIdBytype(string type)
    {
        var nutrientIds = new List<string>();

        const string query = "SELECT NutrientId FROM Nutrients WHERE LOWER(Type) = LOWER(@Type);";

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException(CouldNotCreateDbConnectionMessage);
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        var param = command.CreateParameter();
        param.ParameterName = "@Type";
        param.Value = type;
        command.Parameters.Add(param);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            nutrientIds.Add(reader.GetString(reader.GetOrdinal(NUTRIENT_ID)));
        }

        return nutrientIds;
    }

}
