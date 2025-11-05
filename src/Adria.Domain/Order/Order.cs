namespace Adria.Domain.Order;

public sealed class Order
{
    public Guid OrderId { get; private init; }
    public Guid AdrianId { get; private set; }
    public DateTime Date { get; private set; }
    public double TotalPrice { get; private set; }
    
    public Order(Guid orderId, Guid adrianId, DateTime date, double totalPrice)
    {
        OrderId = orderId;
        AdrianId = adrianId;
        SetDate(date);
        SetTotalPrice(totalPrice);
    }

    public void SetDate(DateTime date)
    {
        Date = date;
    }
    
    public void SetTotalPrice(double totalPrice)
    {
        TotalPrice = EnsureTotalPriceIsValid(totalPrice);
    }
    
    private static double EnsureTotalPriceIsValid(double totalPrice)
    {
        if (double.IsNegative(totalPrice))
            throw new ArgumentException("New price cannot be negative", nameof(totalPrice));
        return totalPrice;
    }
}