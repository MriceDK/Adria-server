using System.Data.Common;
using Adria.Domain.Food;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class FoodRepositories: IFood
{
    private readonly List<Food> _foods = new();
    
    public Task<Food?> ById(string foodId)
    {
        return null;

    }
    
    public Task<IReadOnlyCollection<Food>> ByType(string type)
    {
        return null;

    }

    public async Task<IReadOnlyCollection<Food>> GetAll()
    {
        return null;
    }


    public Task Save(Food food)
    {
        return null;

    }

    public Task Remove(Food food)
    {
        return null;

    }

}