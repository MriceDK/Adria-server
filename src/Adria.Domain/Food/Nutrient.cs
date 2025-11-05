namespace Adria.Domain.Food;

public class Nutrient
{
    public Guid NutrientId { get; private init; }
    public string Type { get; private set; }
    public double RecommendedAmount { get; private set; }

    public Nutrient(Guid nutrientId, string type, double recommendedAmount)
    {
        EnsureTypeIsValid(type);
        EnsureRecommendedAmountIsValid(recommendedAmount);
        NutrientId = nutrientId;
        Type = type;
        RecommendedAmount = recommendedAmount;
    }

    public void ChangeType(string newType)
    {
        EnsureTypeIsValid(newType);
        Type = newType;
    }

    public void ChangeRecommendedAmount(double newAmount)
    {
        EnsureRecommendedAmountIsValid(newAmount);
        RecommendedAmount = newAmount;
    }

    private static void EnsureTypeIsValid(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be null or empty.", nameof(type));
    }

    private static void EnsureRecommendedAmountIsValid(double recommendedAmount)
    {
        if (string.IsNullOrWhiteSpace(recommendedAmount.ToString()))
            throw new ArgumentException("Recommended amount cannot be null or empty.", nameof(recommendedAmount));
    }
}