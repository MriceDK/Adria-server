namespace Adria.Domain.Food;

public class FoodComposition
{
    public Guid FoodId { get; private init; }
    public Guid NutrientId { get; private init; }
    public double Amount { get; private set; }

    public FoodComposition(Guid foodId, Guid nutrientId, double amount)
    {
        EnsureIdIsValid(foodId, nameof(foodId));
        EnsureIdIsValid(nutrientId, nameof(nutrientId));
        EnsureAmountIsValid(amount);
        FoodId = foodId;
        NutrientId = nutrientId;
        Amount = amount;
    }

    public void ChangeAmount(double newAmount)
    {
        EnsureAmountIsValid(newAmount);
        Amount = newAmount;
    }

    private static void EnsureIdIsValid(Guid id, string paramName)
    {
        if (string.IsNullOrWhiteSpace(id.ToString()))
            throw new ArgumentException("ID cannot be null or empty.", paramName);
    }

    private static void EnsureAmountIsValid(double amount)
    {
        if (string.IsNullOrWhiteSpace(amount.ToString()))
            throw new ArgumentException("Amount cannot be null or empty.", nameof(amount));
    }
}