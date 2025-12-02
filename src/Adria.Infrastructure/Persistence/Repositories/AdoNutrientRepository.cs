using System.Data.Common;
using Adria.Domain.Food;
using Adria.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoNutrientRepository : AbstractAdoRepository, INutrient
{
    public AdoNutrientRepository(DbProviderFactory factory, string connectionString)
        : base(factory, connectionString) { }

    public Task<Nutrient?> ById(string nutrientId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Nutrient>> ByType(string type)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Nutrient nutrient)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<Nutrient>> GetAllNutrients()
    {
        var nutrients = new List<Nutrient>();

        using var connection = _factory.CreateConnection() 
            ?? throw new InvalidOperationException("Could not create DB connection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT NutrientId, Type, RecommendedAmount FROM Nutrients";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            nutrients.Add(new Nutrient(
                reader.GetGuid(reader.GetOrdinal("NutrientId")),
                reader.GetString(reader.GetOrdinal("Type")),
                reader.GetDouble(reader.GetOrdinal("RecommendedAmount"))
            ));
        }

        return nutrients;
    }

    public async Task<IReadOnlyCollection<Guid>> GetNutrientIdBytype(string type)
    {
        var nutrientIds = new List<Guid>();

        const string query = "SELECT NutrientId FROM Nutrients WHERE Type = @Type LIMIT 1;";

        using var connection = _factory.CreateConnection() 
            ?? throw new InvalidOperationException("Could not create DB connection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        var param = command.CreateParameter();
        param.ParameterName = "@Type";
        param.Value = type;
        command.Parameters.Add(param);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            nutrientIds.Add(reader.GetGuid(reader.GetOrdinal("NutrientId")));
        }

        return nutrientIds;
    }

    public async Task<Guid> AddNutrient(string type, double recommendedAmount)
    {
        var newId = Guid.NewGuid();

        const string query =
            "INSERT INTO Nutrients (NutrientId, Type, RecommendedAmount) VALUES (@Id, @Type, @RecommendedAmount);";

        using var connection = _factory.CreateConnection() 
            ?? throw new InvalidOperationException("Could not create DB connection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        command.Parameters.Add(CreateParameter("@Id", newId.ToString()));
        command.Parameters.Add(CreateParameter("@Type", type));
        command.Parameters.Add(CreateParameter("@RecommendedAmount", recommendedAmount));

        await command.ExecuteNonQueryAsync();

        return newId;
    }
}