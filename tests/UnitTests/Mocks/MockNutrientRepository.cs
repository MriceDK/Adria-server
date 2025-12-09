using Adria.Domain.Food;

namespace UnitTests.Mocks;

public class MockNutrientRepository:INutrient
{
    private readonly IReadOnlyCollection<Nutrient> _nutrients;

    public MockNutrientRepository(IEnumerable<Nutrient> nutrients)
    {
        _nutrients = nutrients.ToList().AsReadOnly();
    }

    public Task<Nutrient?> ById(string nutrientId)
    {
        return Task.FromResult(_nutrients.FirstOrDefault(n => n.NutrientId.ToString() == nutrientId));
    }

    public Task<IReadOnlyCollection<Nutrient>> ByType(string type)
    {
        return Task.FromResult<IReadOnlyCollection<Nutrient>>(_nutrients.Where(n => n.Type == type).ToList());
    }

    public Task Save(Nutrient nutrient) => Task.CompletedTask;
    public Task Remove(Nutrient nutrient) => Task.CompletedTask;

    public Task<IReadOnlyCollection<Nutrient>> GetAllNutrients()
    {
        return Task.FromResult<IReadOnlyCollection<Nutrient>>(_nutrients);
    }

    public Task<IReadOnlyCollection<string>> GetNutrientIdBytype(string type)
    {
        throw new NotImplementedException();
    }
}