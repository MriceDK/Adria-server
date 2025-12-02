using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace Adria.Application.FoodComposition;

public sealed record CreateFoodCompositionInput(
    Guid FoodId,
    Guid NutrientId,
    double Amount
);

public class CreateFoodComposition : IUseCase<CreateFoodCompositionInput, Task>
{
    private readonly IFoodComposition _repository;

    public CreateFoodComposition(IFoodComposition repository)
    {
        _repository = repository;
    }

    public Task Execute(CreateFoodCompositionInput input)
    {
        var entity = new Domain.Food.FoodComposition(input.FoodId, input.NutrientId, input.Amount);
        return _repository.Save(entity);
    }
}