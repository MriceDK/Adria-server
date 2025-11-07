using System.Data.Common;
using Adria.Domain.Food;
using Adria.Domain.Scanner;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

 public sealed class AdoFoodCompositionRepository : AbstractAdoRepository, IFoodComposition
    {
        private readonly ILogger<AdoFoodCompositionRepository> _logger;

        private static readonly string TABLE_FOOD_COMPOSITION = "FoodCompositions";
        private static readonly string INSERT_FOOD_COMPOSITION = $@"
            INSERT INTO {TABLE_FOOD_COMPOSITION} (FoodId, NutrientId, Amount)
            VALUES (@FoodId, @NutrientId, @Amount);
        ";

        private static readonly string TABLE_SCAN = "Scans";
        private static readonly string INSERT_SCAN = $@"
            INSERT INTO {TABLE_SCAN} (ScanId, AdrianId, DateTime, Result, FoodId)
            VALUES (@ScanId, @AdrianId, @DateTime, @Result, @FoodId);
        ";

        public AdoFoodCompositionRepository(DbProviderFactory factory, string connectionString)
            : base(factory, connectionString)
        {
        }

        public async Task Save(FoodComposition foodComposition)
        {
            try
            {
                bool foodExists = await ExistsInTable("Foods", "FoodId", foodComposition.FoodId);
                if (!foodExists)
                    throw new ArgumentException("Invalid FoodId: does not exist in Foods table.");

                bool nutrientExists = await ExistsInTable("Nutrients", "NutrientId", foodComposition.NutrientId);
                if (!nutrientExists)
                    throw new ArgumentException("Invalid NutrientId: does not exist in Nutrients table.");

                await ExecuteNonQueryAsync(INSERT_FOOD_COMPOSITION, new[]
                {
                    CreateParameter("@FoodId", foodComposition.FoodId.ToString()),
                    CreateParameter("@NutrientId", foodComposition.NutrientId.ToString()),
                    CreateParameter("@Amount", foodComposition.Amount)
                });
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Failed to save FoodComposition.");
                throw;
            }
        }
        
        private async Task<bool> ExistsInTable(string tableName, string columnName, Guid id)
        {
            string query = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @Id";
            using var reader = await ExecuteReaderAsync(query, new[] { CreateParameter("@Id", id.ToString()) });
            if (await reader.ReadAsync())
            {
                return reader.GetInt32(0) > 0;
            }
            return false;
        }

      

        // Additional data retrieval methods can be added here
        public Task<IReadOnlyCollection<FoodComposition>> ByFoodId(string foodId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<FoodComposition>> ByNutrientId(string nutrientId)
        {
            throw new NotImplementedException();
        }
        
        public Task Remove(FoodComposition foodComposition)
        {
            throw new NotImplementedException();
        }
    }