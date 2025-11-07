namespace Adria.Domain.Food;

public interface IFood
{
   Task<IReadOnlyCollection<Food>> ByType(string type);
   Task<IReadOnlyCollection<Food>> GetAll();
   Task Remove(Food food);
   Task<IReadOnlyCollection<Guid>> GetFoodIdByName(string name);
   Task<Guid> AddFood(string name, string type, bool edible);
}