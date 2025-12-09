using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace Adria.Application.FoodComposition;

public class GetAllFoods
{
    private readonly IFood _repository;

    public GetAllFoods(IFood repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<FoodData>> Execute()
    {
        var foods = await _repository.GetAll();

        var result = foods
            .Select(f => new FoodData(
                FoodId: f.FoodId,
                Name: f.Name,
                Type: f.Type,
                Edible: f.Edible
            ))
            .ToArray();

        return result;
    }
}