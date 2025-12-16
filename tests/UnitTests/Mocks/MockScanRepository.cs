using Adria.Domain.Scanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockScanRepository : IScan
{
    private readonly List<Scan> _scans = new();

    public Task Save(Scan scan)
    {
        _scans.Add(scan);
        return Task.CompletedTask;
    }

    public Task Remove(Guid scanId)
    {
        _scans.RemoveAll(s => s.ScanId == scanId);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<Scan>> ByUserId(Guid adrianId)
    {
        return Task.FromResult<IReadOnlyCollection<Scan>>(
            _scans.Where(s => s.AdrianId == adrianId).ToList()
        );
    }

    public Task<Scan?> ById(Guid scanId)
    {
        return Task.FromResult(
            _scans.FirstOrDefault(s => s.ScanId == scanId)
        );
    }
}