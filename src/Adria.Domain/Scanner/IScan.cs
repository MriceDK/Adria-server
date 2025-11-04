namespace Adria.Domain.Scanner;

public interface IScan
{
    Task<Scan?> ById(string scanId);
    Task<IReadOnlyCollection<Scan>> ByUserId(string adrianId);
    Task Save(Scan scan);
    Task Remove(Scan scan);
}