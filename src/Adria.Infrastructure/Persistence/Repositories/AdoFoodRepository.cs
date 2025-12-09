using System.Data.Common;
using Adria.Domain.Food;
using Adria.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoFoodRepository : AbstractAdoRepository, IFood
{
    public AdoFoodRepository(DbProviderFactory factory, string connectionString)
        : base(factory, connectionString)
    {
    }

    public async Task Save(Food food)
    {
        const string query = @"INSERT INTO foods (FoodId, Name, Type, Edible)
                           VALUES (@FoodId, @Name, @Type, @Edible);";

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException("Could not create DB connection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.Add(CreateParameter("@FoodId", food.FoodId.ToString()));
        command.Parameters.Add(CreateParameter("@Name", food.Name));
        command.Parameters.Add(CreateParameter("@Type", food.Type));
        command.Parameters.Add(CreateParameter("@Edible", food.Edible));

        var affected = await command.ExecuteNonQueryAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row into foods, but affected {affected}.");
        }
    }
    public async Task<Food?> ById(Guid id)
    {
        using var connection = _factory.CreateConnection();
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT FoodId, Name, Type, Edible FROM foods WHERE FoodId = @Id";

        var param = command.CreateParameter();
        param.ParameterName = "@Id";
        param.Value = id.ToString();
        command.Parameters.Add(param);

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Food(
            reader.GetGuid(reader.GetOrdinal("FoodId")),
            reader.GetString(reader.GetOrdinal("Name")),
            reader.GetString(reader.GetOrdinal("Type")),
            reader.GetBoolean(reader.GetOrdinal("Edible"))
        );
    }


    public async Task<IReadOnlyCollection<Food>> GetAll()
    {
        var foods = new List<Food>();

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException("Could not create DB connection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT FoodId, Name, Type, Edible FROM foods";

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
    public Task Remove(Food food)
        => throw new NotImplementedException();

    public async Task<IReadOnlyCollection<Guid>> GetFoodIdByName(string name)
    {
        var ids = new List<Guid>();

        const string query = "SELECT FoodId FROM Foods WHERE Name = @Name LIMIT 1;";

        using var connection = _factory.CreateConnection()
                           ?? throw new InvalidOperationException("Could not create DB connection.");
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

        using var connection = _factory.CreateConnection()
                           ?? throw new InvalidOperationException("Could not create DB connection.");
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
