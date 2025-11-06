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

    
    public async Task<Food?> ById(string foodId)
        => throw new NotImplementedException();

    public async Task<IReadOnlyCollection<Food>> ByType(string type)
        => throw new NotImplementedException();
    

    public async Task Save(Food food)
        => throw new NotImplementedException();

    public async Task Remove(Food food)
        => throw new NotImplementedException();
    
    
}