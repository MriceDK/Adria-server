using Adria.Application.Contracts.Data;
using Adria.Domain.Food;
using Adria.Domain.Scanner;


namespace Adria.Application.Scanner
{
    public sealed class ScanFood
    {
        private readonly GetRandomFood _getRandomFood;
        private readonly GetFoodNutrients _getFoodNutrients;
        private readonly IScan _scanRepository;

        public ScanFood(GetRandomFood getRandomFood, GetFoodNutrients getFoodNutrients, IScan scanRepository)
        {
            _getRandomFood = getRandomFood;
            _getFoodNutrients = getFoodNutrients;
            _scanRepository = scanRepository;
        }

        public async Task<ScannedFoodResult> Execute(Guid adrianId)
        {
            Food food = await _getRandomFood.Execute();
            IReadOnlyCollection<NutrientInfo> nutrients = await _getFoodNutrients.Execute(food.FoodId);

            Guid scanId = Guid.NewGuid();
            DateTime scanDateTime = DateTime.UtcNow;

            Scan scan = new Scan
            (
                scanId,
                adrianId,
                scanDateTime,
                $"Scanned food: {food.Name}",
                food.FoodId.ToString()
            );
            await _scanRepository.Save(scan);
            
            return new ScannedFoodResult(
                scanId,
                adrianId,
                food.FoodId,
                food.Name,
                food.Type,
                food.Edible,
                nutrients,
                scanDateTime
            );
        }
    }
}