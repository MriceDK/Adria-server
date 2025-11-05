namespace Adria.Application.Contracts.Data;

public sealed record SubscriptionData(
    Guid SubscriptionId,
    Type Type,
    double PricePerMonth,
    string Advantages
);
