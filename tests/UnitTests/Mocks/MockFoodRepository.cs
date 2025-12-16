using Adria.Domain.Food;

namespace UnitTests.Mocks;

public sealed class MockFoodRepository : IFood
{
    private readonly List<Food> _foods;

    public IReadOnlyList<Food> SavedEntities => _foods;

    public MockFoodRepository()
    {
        _foods = new List<Food>();
    }

    public MockFoodRepository(IEnumerable<Food> foods)
    {
        _foods = foods.ToList();
    }

    public Task Save(Food food)
    {
        _foods.Add(food);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Food>> GetAll()
    {
        return Task.FromResult<IReadOnlyCollection<Food>>(_foods.AsReadOnly());
    }

    public Task<Food?> ById(Guid id)
    {
        return Task.FromResult<Food?>(_foods.FirstOrDefault(f => f.FoodId == id));
    }

    public Task<Food?> ById(string foodId)
    {
        return Task.FromResult<Food?>(
            _foods.FirstOrDefault(f => f.FoodId.ToString() == foodId)
        );
    }

    public Task Remove(Food food)
    {
        _foods.Remove(food);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Guid>> GetFoodIdByName(string name)
    {
        var ids = _foods
            .Where(f => f.Name == name)
            .Select(f => f.FoodId)
            .ToList()
            .AsReadOnly();

        return Task.FromResult<IReadOnlyCollection<Guid>>(ids);
    }

    public Task<Guid> AddFood(string name, string type, bool edible)
    {
        var id = Guid.NewGuid();
        _foods.Add(new Food(id, name, type, edible));
        return Task.FromResult(id);
    }
}