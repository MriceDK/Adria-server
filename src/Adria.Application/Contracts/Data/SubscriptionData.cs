using Adria.Domain.Subcriptions;

namespace Adria.Application.Contracts.Data;

public sealed record SubscriptionData(
    Guid SubscriptionId,
    SubscriptionType Type,
    double PricePerMonth,
    string Advantages
);