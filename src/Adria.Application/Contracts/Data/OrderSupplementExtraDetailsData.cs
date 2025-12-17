namespace Adria.Application.Contracts.Data;

public sealed record OrderSupplementExtraDetailsData(
    Guid SupplementId,
    string Name,
    string Type,
    double Price,
    int Amount
);