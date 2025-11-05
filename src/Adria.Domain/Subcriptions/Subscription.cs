namespace Adria.Domain.Subcriptions;

public sealed class Subscription
{
    public Subscription(SubscriptionType subscriptionType, double pricePerMonth, string advantages, Guid subscriptionId = default)
    {
        EnsureAdvantagesIsNotEmpty(advantages);
        
        Id = subscriptionId == Guid.Empty ? Guid.NewGuid() : subscriptionId;
        SubscriptionType = subscriptionType;
        PricePerMonth = pricePerMonth;
        Advantages = advantages;
    }

    public Guid Id { get; private init; }
    public SubscriptionType SubscriptionType { get; set; }
    public double PricePerMonth { get; set; }
    public string Advantages { get; set; }


    private static void EnsureAdvantagesIsNotEmpty(string advantages)
    {
        if (string.IsNullOrWhiteSpace(advantages))
            throw new ArgumentException("Advantages cannot be null or empty.", nameof(advantages));
    }
}