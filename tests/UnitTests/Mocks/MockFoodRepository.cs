using Adria.Domain.Food;

namespace UnitTests.Mocks;

public class MockFoodRepository : IFood
{
    private readonly IReadOnlyCollection<Food> _foods;

    public MockFoodRepository(IReadOnlyCollection<Food> foods)
    {
        _foods = foods;
    }

    public Task<IReadOnlyCollection<Food>> GetAll()
    {
        return Task.FromResult(_foods);
    }

    public Task<Food?> ById(string foodId)
    {
        foreach (Food food in _foods)
        {
            if (food.FoodId.ToString() == foodId)
                return Task.FromResult<Food?>(food);
        }
        return Task.FromResult<Food?>(null);
    }

    public Task<IReadOnlyCollection<Food>> ByType(string type)
    {
        List<Food> result = new List<Food>();
        foreach (Food food in _foods)
        {
            if (food.Type == type)
                result.Add(food);
        }
        return Task.FromResult<IReadOnlyCollection<Food>>(result);
    }

    public Task Save(Food food) => Task.CompletedTask;
    public Task Remove(Food food) => Task.CompletedTask;
}