namespace Adria.Domain.Order;

public sealed class Supplement
{
    public Guid SupplementId { get; private init; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public double Price { get; private set; }
    public int Stock { get; private set; }
    
    public Supplement(Guid supplementId, string name, string type, double price, int stock)
    {
        SupplementId = supplementId;
        Name = EnsureNameIsValid(name);
        Type = EnsureNameIsValid(type);
        Price = EnsurePriceIsValid(price);
        Stock = EnsureStockIsValid(stock);
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
    
    public void SetStock(int stock)
    {
        Stock = EnsureStockIsValid(stock);
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
    
    private static int EnsureStockIsValid(int stock)
    {
        if (stock < 0)
            throw new ArgumentException("New stock cannot be below 0.", nameof(stock));
        return stock;
    }
}