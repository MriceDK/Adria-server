namespace Adria.Application.Contracts.Data;

public sealed record OrderData(
    Guid OrderId,
    Guid AdrianId,
    DateTime Date,
    double TotalPrice
);