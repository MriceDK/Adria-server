using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Food;

namespace Adria.Application.Food;

public class CreateNewFood : IUseCase<FoodData, Task<Guid>>
{
    private readonly IFood _repository;

    public CreateNewFood(IFood repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Execute(FoodData input)
    {
        var id = Guid.NewGuid();
        
        var entity = new Domain.Food.Food(
            id,
            input.Name,
            input.Type,
            input.Edible
        );

        await _repository.Save(entity);

        return id; 
    }
}