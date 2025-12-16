using Adria.Domain.Food;

namespace UnitTests.Mocks;

public sealed class MockFoodCompositionRepository : IFoodComposition
{
    private readonly IReadOnlyCollection<FoodComposition> _comps;
    public List<FoodComposition> SavedEntities { get; } = new();

    public MockFoodCompositionRepository()
        : this(Array.Empty<FoodComposition>())
    {
    }

    public MockFoodCompositionRepository(IEnumerable<FoodComposition> comps)
    {
        _comps = comps.ToList().AsReadOnly();
    }

    public Task Save(FoodComposition entity)
    {
        SavedEntities.Add(entity);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<FoodComposition>> ByFoodId(string foodId)
    {
        return Task.FromResult<IReadOnlyCollection<FoodComposition>>(
            _comps.Where(c => c.FoodId.ToString() == foodId).ToList()
        );
    }

    public Task<IReadOnlyCollection<FoodComposition>> ByNutrientId(string nutrientId)
    {
        return Task.FromResult<IReadOnlyCollection<FoodComposition>>(
            _comps.Where(c => c.NutrientId == nutrientId).ToList()
        );
    }

    public Task Remove(FoodComposition foodComposition)
    {
        return Task.CompletedTask;
    }
}