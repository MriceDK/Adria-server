namespace Adria.Domain.Food;

public class Nutrient
{
    public string NutrientId { get; private init; }
    public string Type { get; private set; }
    public string Unit { get; private set; }

    public Nutrient(string nutrientId, string type, string unit)
    {
        EnsureTypeIsValid(type);
        NutrientId = nutrientId;
        Type = type;
        Unit = unit;
    }

    public void ChangeType(string newType)
    {
        EnsureTypeIsValid(newType);
        Type = newType;
    }



    private static void EnsureTypeIsValid(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be null or empty.", nameof(type));
    }


}