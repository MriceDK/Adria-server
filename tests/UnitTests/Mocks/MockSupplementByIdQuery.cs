using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockSupplementsByIdQuery : ISupplementsByIdQuery
{
    private SupplementData? _returnValue;

    public void SetReturnValue(SupplementData? value)
    {
        _returnValue = value;
    }

    public Task<SupplementData?> Fetch(Guid id)
    {
        return Task.FromResult(_returnValue);
    }
}