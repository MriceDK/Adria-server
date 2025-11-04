using Adria.Domain.Food;

namespace UnitTests.Mocks;

public class MockFoodCompositionRepository : IFoodComposition
{
    private readonly IReadOnlyCollection<FoodComposition> _comps;

    public MockFoodCompositionRepository(IEnumerable<FoodComposition> comps)
    {
        _comps = comps.ToList().AsReadOnly();
    }

    public Task<IReadOnlyCollection<FoodComposition>> ByFoodId(string foodId)
    {
        return Task.FromResult<IReadOnlyCollection<FoodComposition>>(_comps.Where(c => c.FoodId.ToString() == foodId).ToList());
    }

    public Task<IReadOnlyCollection<FoodComposition>> ByNutrientId(string nutrientId)
    {
        return Task.FromResult<IReadOnlyCollection<FoodComposition>>(_comps.Where(c => c.NutrientId.ToString() == nutrientId).ToList());
    }

    public Task Save(FoodComposition foodComposition) => Task.CompletedTask;
    public Task Remove(FoodComposition foodComposition) => Task.CompletedTask;
}