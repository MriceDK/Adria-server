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
        CreateParameter("@FoodId", foodComposition.FoodId.ToString()),
        CreateParameter("@NutrientId", foodComposition.NutrientId.ToString()),
        CreateParameter("@Amount", foodComposition.Amount)
    ];

    private async Task<bool> ExistsInTable(string table, string column, Guid id)
    {
        string query = $"SELECT COUNT(1) FROM {table} WHERE {column} = @Id";

        using DbDataReader reader = await ExecuteReaderAsync(
            query,
            [CreateParameter("@Id", id.ToString())]
        );

        if (await reader.ReadAsync())
            return reader.GetInt32(0) > 0;

        return false;
    }

    public async Task<IReadOnlyCollection<FoodComposition>> ByFoodId(string foodName)
    {
        var ids = await _foodRepository.GetFoodIdByName(foodName);

        if (ids.Count == 0)
        {
            _logger.LogInformation("No FoodId found for food name {FoodName}.", foodName);
            return Array.Empty<FoodComposition>();
        }

        var foodId = ids.First();

        try
        {
            _logger.LogInformation("Reading FoodComposition entries for FoodId {FoodId}", foodId);

            DbParameter foodParam = CreateParameter("@FoodId", foodId.ToString());
            using DbDataReader reader = await ExecuteReaderAsync(SELECT_BY_FOOD_ID, [foodParam]);

            var result = new List<FoodComposition>();

            while (await reader.ReadAsync())
            {
                result.Add(new FoodComposition(
                    Guid.Parse(reader.GetString(reader.GetOrdinal(COL_FOOD_ID))),
                    Guid.Parse(reader.GetString(reader.GetOrdinal(COL_NUTRIENT_ID))),
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
        if (string.IsNullOrWhiteSpace(type))
        {
            _logger.LogInformation("Type was null or empty.");
            return Array.Empty<FoodComposition>();
        }

        var nutrientIds = await _nutrientRepository.GetNutrientIdBytype(type) 
                          ?? Array.Empty<Guid>();

        if (nutrientIds.Count == 0)
        {
            _logger.LogInformation("No NutrientId found for type {Type}.", type);
            return Array.Empty<FoodComposition>();
        }

        var nutrientId = nutrientIds.First();

        try
        {
            _logger.LogInformation("Reading FoodComposition entries for NutrientId {NutrientId}", nutrientId);

            DbParameter nutriParam = CreateParameter("@NutrientId", nutrientId.ToString());
            using DbDataReader reader = await ExecuteReaderAsync(SELECT_BY_NUTRIENT_ID, new[] { nutriParam });

            var result = new List<FoodComposition>();

            while (await reader.ReadAsync())
            {
                result.Add(new FoodComposition(
                    Guid.Parse(reader.GetString(reader.GetOrdinal(COL_FOOD_ID))),
                    Guid.Parse(reader.GetString(reader.GetOrdinal(COL_NUTRIENT_ID))),
                    reader.GetDouble(reader.GetOrdinal(COL_AMOUNT))
                ));
            }

            return result;
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to read FoodComposition entries for NutrientId {NutrientId}", nutrientId);
            throw new NutriscanDatabaseException("Database error reading food compositions.", ex);
        }
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
                    CreateParameter("@FoodId", foodComposition.FoodId.ToString()),
                    CreateParameter("@NutrientId", foodComposition.NutrientId.ToString())
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
