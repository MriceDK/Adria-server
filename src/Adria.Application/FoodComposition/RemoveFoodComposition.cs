using Adria.Application.Contracts;
using Adria.Domain.Food;

namespace Adria.Application.FoodComposition;


public sealed record RemoveFoodCompositionInput(
    Guid FoodId,
    Guid NutrientId,
    double Amount
);


public class RemoveFoodComposition
    : IUseCase<RemoveFoodCompositionInput, Task>
{
    private readonly IFoodComposition _repository;

    public RemoveFoodComposition(IFoodComposition repository)
    {
        _repository = repository;
    }

    public Task Execute(RemoveFoodCompositionInput input)
    {
        var entity = new Domain.Food.FoodComposition(input.FoodId, input.NutrientId, input.Amount);
        return _repository.Remove(entity);
    }
}