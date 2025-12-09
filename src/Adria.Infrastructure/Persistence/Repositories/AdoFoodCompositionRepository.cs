using System.Data.Common;
using Adria.Domain.Food;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public sealed class AdoFoodCompositionRepository : AbstractAdoRepository, IFoodComposition
{
    private readonly ILogger<AdoFoodCompositionRepository> _logger;
    private readonly IFood _foodRepository;
    private readonly INutrient _nutrientRepository;

    private static readonly string TABLE_FOOD_COMPOSITION = "foodCompositions";
    private static readonly string TABLE_FOODS = "foods";
    private static readonly string TABLE_NUTRIENTS = "nutrients";

    private static readonly string COL_FOOD_ID = "FoodId";
    private static readonly string COL_NUTRIENT_ID = "NutrientId";
    private static readonly string COL_AMOUNT = "Amount";

    private static readonly string INSERT_FOOD_COMPOSITION = $@"
        INSERT INTO {TABLE_FOOD_COMPOSITION} ({COL_FOOD_ID}, {COL_NUTRIENT_ID}, {COL_AMOUNT})
        VALUES (@FoodId, @NutrientId, @Amount);
    ";

    private static readonly string DELETE_FOOD_COMPOSITION = $@"
        DELETE FROM {TABLE_FOOD_COMPOSITION}
        WHERE {COL_FOOD_ID} = @FoodId 
          AND {COL_NUTRIENT_ID} = @NutrientId;
    ";

    private static readonly string SELECT_BY_FOOD_ID = $@"
        SELECT {COL_FOOD_ID}, {COL_NUTRIENT_ID}, {COL_AMOUNT}
        FROM {TABLE_FOOD_COMPOSITION}
        WHERE {COL_FOOD_ID} = @FoodId;
    ";

    private static readonly string SELECT_BY_NUTRIENT_ID = $@"
        SELECT {COL_FOOD_ID}, {COL_NUTRIENT_ID}, {COL_AMOUNT}
        FROM {TABLE_FOOD_COMPOSITION}
        WHERE {COL_NUTRIENT_ID} = @NutrientId;
    ";

    public AdoFoodCompositionRepository(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoFoodCompositionRepository> logger,
        IFood foodRepository,
        INutrient nutrientRepository
    ) : base(factory, connectionString)
    {
        _logger = logger;
        _foodRepository = foodRepository;
        _nutrientRepository = nutrientRepository;
    }

    public async Task Save(FoodComposition foodComposition)
    {
        try
        {
            _logger.LogInformation(
                "Saving FoodComposition for Food {FoodId} and Nutrient {NutrientId}.",
                foodComposition.FoodId,
                foodComposition.NutrientId
            );

            if (!await ExistsInTable(TABLE_FOODS, COL_FOOD_ID, foodComposition.FoodId))
                throw new ArgumentException("Invalid FoodId: does not exist in foods table.");

            if (!await ExistsInTable(TABLE_NUTRIENTS, COL_NUTRIENT_ID, foodComposition.NutrientId))
                throw new ArgumentException("Invalid NutrientId: does not exist in nutrients table.");

            await ExecuteNonQueryAsync(
                INSERT_FOOD_COMPOSITION,
                CreateParameters(foodComposition)
            );
        }
        catch (DbException ex)
        {
            _logger.LogError(ex,
                "Failed to save FoodComposition for Food {FoodId} and Nutrient {NutrientId}.",
                foodComposition.FoodId,
                foodComposition.NutrientId);

            throw new NutriscanDatabaseException("Failed to save FoodComposition.", ex);
        }
    }

    private DbParameter[] CreateParameters(FoodComposition foodComposition) =>
    [
        CreateParameter("@FoodId", foodComposition.FoodId),
        CreateParameter("@NutrientId", foodComposition.NutrientId),
        CreateParameter("@Amount", foodComposition.Amount)
    ];

    private async Task<bool> ExistsInTable(string table, string column, object id)
    {
        string query = $"SELECT COUNT(1) FROM {table} WHERE {column} = @Id";

        var param = CreateParameter("@Id", id);

        using DbDataReader reader = await ExecuteReaderAsync(query, new[] { param });

        if (await reader.ReadAsync())
            return reader.GetInt32(0) > 0;

        return false;
    }

    public async Task<IReadOnlyCollection<FoodComposition>> ByFoodId(string foodKey)
    {
        Guid foodId;

        if (Guid.TryParse(foodKey, out var parsedId))
        {
            foodId = parsedId;
        }
        else
        {
            var ids = await _foodRepository.GetFoodIdByName(foodKey);

            if (ids.Count == 0)
            {
                _logger.LogInformation("No FoodId found for food name {FoodName}.", foodKey);
                return Array.Empty<FoodComposition>();
            }

            foodId = ids.First();
        }

        try
        {
            _logger.LogInformation("Reading FoodComposition entries for FoodId {FoodId}", foodId);

            DbParameter foodParam = CreateParameter("@FoodId", foodId);
            using DbDataReader reader = await ExecuteReaderAsync(SELECT_BY_FOOD_ID, new[] { foodParam });

            var result = new List<FoodComposition>();

            while (await reader.ReadAsync())
            {
                result.Add(new FoodComposition(
                    reader.GetGuid(reader.GetOrdinal(COL_FOOD_ID)),
                    reader.GetString(reader.GetOrdinal(COL_NUTRIENT_ID)),
                    reader.GetDouble(reader.GetOrdinal(COL_AMOUNT))
                ));
            }

            return result;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read FoodComposition entries for FoodId {FoodId}", foodId);
            throw new NutriscanDatabaseException("Database error reading food compositions.", ex);
        }
    }


    public async Task<IReadOnlyCollection<FoodComposition>> ByNutrientId(string type)
    {
        var nutrientIds = await _nutrientRepository.GetNutrientIdBytype(type)
                          ?? Array.Empty<string>();

        if (nutrientIds.Count == 0)
            return Array.Empty<FoodComposition>();

        var nutrientId = nutrientIds.First();

        const string query = @"
        SELECT FoodId, NutrientId, Amount
        FROM foodCompositions
        WHERE NutrientId = @NutrientId;
    ";

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException("Could not create DB connection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.Add(CreateParameter("@NutrientId", nutrientId));

        using var reader = await command.ExecuteReaderAsync();

        var result = new List<FoodComposition>();

        while (await reader.ReadAsync())
        {
            result.Add(new FoodComposition(
                reader.GetGuid(reader.GetOrdinal("FoodId")),
                reader.GetString(reader.GetOrdinal("NutrientId")),
                reader.GetDouble(reader.GetOrdinal("Amount"))
            ));
        }

        return result;
    }


    public async Task Remove(FoodComposition foodComposition)
    {
        try
        {
            _logger.LogInformation(
                "Removing FoodComposition for Food {FoodId} and Nutrient {NutrientId}.",
                foodComposition.FoodId,
                foodComposition.NutrientId
            );

            await ExecuteNonQueryAsync(
                DELETE_FOOD_COMPOSITION,
                [
                    CreateParameter("@FoodId", foodComposition.FoodId),
                    CreateParameter("@NutrientId", foodComposition.NutrientId)
                ]
            );
        }
        catch (DbException ex)
        {
            _logger.LogError(ex,
                "Failed to remove FoodComposition for Food {FoodId} and Nutrient {NutrientId}.",
                foodComposition.FoodId,
                foodComposition.NutrientId);

            throw new NutriscanDatabaseException("Failed to remove FoodComposition.", ex);
        }
    }
}
