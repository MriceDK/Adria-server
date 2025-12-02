namespace Adria.Application.Contracts.Data;

public sealed record OrderSupplementDetailsData(
    Guid OrderId, 
    Guid SupplementId,
    int Amount
    );