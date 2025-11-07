using System.Data.Common;
using Adria.Domain.Food;
using Adria.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoFoodRepository : AbstractAdoRepository, IFood
{
    public AdoFoodRepository(DbProviderFactory factory, string connectionString)
        : base(factory, connectionString) // <-- Call the base constructor
    {
    }
    public async Task<IReadOnlyCollection<Food>> GetAll()
    {
        var foods = new List<Food>();

        using var connection = _factory.CreateConnection();
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT FoodId, Name, Type, Edible FROM Foods";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            foods.Add(new Food(
                reader.GetGuid(reader.GetOrdinal("FoodId")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("Type")),
                reader.GetBoolean(reader.GetOrdinal("Edible"))
            ));
        }

        return foods;
    }
    
    public async Task<IReadOnlyCollection<Food>> ByType(string type)
        => throw new NotImplementedException();
    

    public async Task Remove(Food food)
        => throw new NotImplementedException();

    public async Task<IReadOnlyCollection<Guid>> GetFoodIdByName(string name)
    {
        var ids = new List<Guid>();

        const string query = "SELECT FoodId FROM Foods WHERE Name = @Name LIMIT 1;";

        using var connection = _factory.CreateConnection();
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;

        var param = command.CreateParameter();
        param.ParameterName = "@Name";
        param.Value = name;
        command.Parameters.Add(param);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            ids.Add(reader.GetGuid(reader.GetOrdinal("FoodId")));
        }

        return ids;
    }
    
    public async Task<Guid> AddFood(string name, string type, bool edible)
    {
        var newId = Guid.NewGuid();
        const string query = "INSERT INTO Foods (FoodId, Name, Type, Edible) VALUES (@Id, @Name, @Type, @Edible);";
        using var connection = _factory.CreateConnection();
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.Add(CreateParameter("@Id", newId.ToString()));
        command.Parameters.Add(CreateParameter("@Name", name));
        command.Parameters.Add(CreateParameter("@Type", type));
        command.Parameters.Add(CreateParameter("@Edible", edible));
        await command.ExecuteNonQueryAsync();
        return newId;
    }
}