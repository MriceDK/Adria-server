using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace Adria.Application.FoodComposition;


public sealed record SearchFoodCompositionsByFoodIdInput(string name);


public sealed class SearchFoodCompositionsByFoodName : IUseCase<SearchFoodCompositionsByFoodIdInput, Task<IReadOnlyCollection<FoodCompositionData>>>
{
    private readonly IFoodComposition _repository;

    public SearchFoodCompositionsByFoodName(IFoodComposition repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<FoodCompositionData>> Execute(SearchFoodCompositionsByFoodIdInput input)
    {
        IReadOnlyCollection<Domain.Food.FoodComposition> result = await _repository.ByFoodId(input.name);
        return result
            .Select(x => new FoodCompositionData(x.FoodId, x.NutrientId, x.Amount))
            .ToArray();
    }
}