namespace Adria.Domain.Food;

public interface IFood
{
   Task Save(Food food);
   Task<IReadOnlyCollection<Food>> GetAll();
   Task Remove(Food food);
   Task<IReadOnlyCollection<Guid>> GetFoodIdByName(string name);
   Task<Guid> AddFood(string name, string type, bool edible);
   Task<Food?> ById(Guid id);

}