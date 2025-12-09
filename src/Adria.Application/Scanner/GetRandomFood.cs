using System.Security.Cryptography;
using Adria.Application.Contracts;
using Adria.Domain.Food;

namespace Adria.Application.Scanner;

public sealed class GetRandomFood: IUseCase <Task<Domain.Food.Food>>
{
    private readonly IFood _food;


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
        

        return foodsList[RandomNumberGenerator.GetInt32(foodsList.Count)];
    }
}