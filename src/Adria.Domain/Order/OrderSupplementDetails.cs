namespace Adria.Domain.Order;

public sealed class OrderSupplementDetails
{
    public Guid OrderId { get; private init; }
    public Guid SupplementId { get; private set; }
    public int Amount { get; private set; }
    
    public OrderSupplementDetails(Guid orderId, Guid supplementId, int amount)
    {
        OrderId = orderId;
        SupplementId = supplementId;
        SetAmount(amount);
    }
    
    public void SetAmount(int amount)
    {
        Amount = EnsureAmountIsValid(amount);
    }
    
    private static int EnsureAmountIsValid(int amount)
    {
        if (int.IsNegative(amount))
            throw new ArgumentException("New price cannot be negative", nameof(amount));
        return amount;
    }
}