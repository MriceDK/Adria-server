namespace Adria.Domain.Scanner;

public interface IScan
{
    Task<Scan?> ById(Guid scanId);
    Task<IReadOnlyCollection<Scan>> ByUserId(Guid adrianId);
    Task Save(Scan scan);
    Task Remove(Scan scan);
}