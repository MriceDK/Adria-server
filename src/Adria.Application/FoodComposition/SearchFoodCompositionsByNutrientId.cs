using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace Adria.Application.FoodComposition;

public sealed record SearchFoodCompositionsByNutrientIdInput(string Type);


public class SearchFoodCompositionsByNutrientId
    : IUseCase<SearchFoodCompositionsByNutrientIdInput, Task<IReadOnlyCollection<FoodCompositionData>>>
{
    private readonly IFoodComposition _repository;

    public SearchFoodCompositionsByNutrientId(IFoodComposition repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<FoodCompositionData>> Execute(SearchFoodCompositionsByNutrientIdInput input)
    {
        IReadOnlyCollection<Domain.Food.FoodComposition> result = await _repository.ByNutrientId(input.Type);
        return result
            .Select(x => new FoodCompositionData(x.FoodId, x.NutrientId, x.Amount))
            .ToArray();
    }
}