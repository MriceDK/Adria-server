namespace Adria.Application.Contracts.Data;

public sealed record SupplementData(
    Guid SupplementId,
    string Name,
    string Type,
    double Price,
    int Stock
);