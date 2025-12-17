using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockSupplementsByNameQuery : ISupplementsByNameQuery
{
    private IReadOnlyCollection<SupplementData?>? _returnValue;

    public void SetReturnValue(IReadOnlyCollection<SupplementData?>? value)
    {
        _returnValue = value;
    }

    public Task<IReadOnlyCollection<SupplementData>> Fetch(string name)
    {
        return Task.FromResult(
            (IReadOnlyCollection<SupplementData>)(_returnValue ?? Array.Empty<SupplementData>())
        );
    }

}