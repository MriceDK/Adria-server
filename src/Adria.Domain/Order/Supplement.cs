namespace Adria.Domain.Order;

public sealed class Supplement
{
    public Guid SupplementId { get; private init; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public double Price { get; private set; }
    
    public Supplement(Guid supplementId, string name, string type, double price)
    {
        SupplementId = supplementId;
        SetName(name);
        SetType(type);
        SetPrice(price);
    }

    public void SetName(string name)
    {
        Name = EnsureNameIsValid(name);
    }
    
    public void SetType(string type)
    {
        Type = EnsureNameIsValid(type);
    }
    
    public void SetPrice(double price)
    {
        Price = EnsurePriceIsValid(price);
    }
    
    private static double EnsurePriceIsValid(double price)
    {
        if (double.IsNegative(price))
            throw new ArgumentException("New price cannot be negative", nameof(price));
        return price;
    }
    
    private static string EnsureNameIsValid(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        return name;
    }
}