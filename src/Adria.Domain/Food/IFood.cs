namespace Adria.Domain.Food;

public interface IFood
{
   Task<Food?> ById(string foodId);
   Task<IReadOnlyCollection<Food>> ByType(string type);
   Task<IReadOnlyCollection<Food>> GetAll();
   Task Save(Food food);
   Task Remove(Food food);
}