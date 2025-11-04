using Adria.Application.Contracts;
using Adria.Domain.Food;

namespace Adria.Application.Scanner;

public sealed class GetRandomFood: IUseCase <Task<Food>>
{
    private readonly IFood _food;
    
    public GetRandomFood(IFood food)
    {
        _food = food;
    }

    public async Task<Food> Execute()
    {
        IReadOnlyCollection<Food> allFoods = await _food.GetAll();
        if (allFoods == null || allFoods.Count == 0)
            throw new InvalidOperationException("No foods available.");
        List<Food> foodsList = new List<Food>(allFoods);
        Random random = new Random();
        int i = random.Next(foodsList.Count);
        return foodsList[i];
    }
}