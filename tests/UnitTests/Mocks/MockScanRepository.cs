using Adria.Domain.Scanner;

namespace UnitTests.Mocks;

public class MockScanRepository : IScan
{
    private readonly List<Scan> _scans = new();

    public Task Save(Scan scan)
    {
        _scans.Add(scan);
        return Task.CompletedTask;
    }

    public Task Remove(Guid scanId)
    {
        throw new NotImplementedException();
    }


    public Task<IReadOnlyCollection<Scan>> ByUserId(Guid adrianId)
    {
        var result = _scans.Where(s => s.AdrianId == adrianId).ToList();
        return Task.FromResult<IReadOnlyCollection<Scan>>(result);
    }

    public Task<Scan?> ById(Guid scanId)
    {
        var scan = _scans.FirstOrDefault(s => s.ScanId == scanId);
        return Task.FromResult(scan);
    }
}