namespace Adria.Domain.Food;

public sealed class Food
{
    public Guid FoodId { get; private init; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public bool Edible { get; private set; }
    
    public Food(Guid foodId, string name, string type, bool edible)
    {
        EnsureFoodNameIsValid(name);
        EnsureFoodTypeIsValid(type);
        FoodId = foodId;
        Name = name;
        Type = type;
        Edible = edible;
    }

    public void ChangeName(string newName)
    {
        EnsureFoodNameIsValid(newName);
        Name = newName;
    }

    public void ChangeType(string newType)
    {
        EnsureFoodTypeIsValid(newType);
        Type = newType;
    }

    public void SetEdible(bool isEdible)
    {
        Edible = isEdible;
    }

    private static void EnsureFoodNameIsValid(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
    }

    private static void EnsureFoodTypeIsValid(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type cannot be null or empty.", nameof(type));
    }
}