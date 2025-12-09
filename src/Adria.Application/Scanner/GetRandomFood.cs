using Adria.Application.Contracts;
using Adria.Domain.Food;

namespace Adria.Application.Scanner;

public sealed class GetRandomFood: IUseCase <Task<Domain.Food.Food>>
{
    private readonly IFood _food;
    private static readonly Random _random = new Random();

    
    public GetRandomFood(IFood food)
    {
        _food = food;
    }

    public async Task<Domain.Food.Food> Execute()
    {
        IReadOnlyCollection<Domain.Food.Food> allFoods = await _food.GetAll();
        if (allFoods == null || allFoods.Count == 0)
            throw new InvalidOperationException("No foods available.");

        List<Domain.Food.Food> foodsList = new List<Domain.Food.Food>(allFoods);

        int i;
        lock (_random)
        {
            i = _random.Next(foodsList.Count);
        }

        return foodsList[i];
    }
}