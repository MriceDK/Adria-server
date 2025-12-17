using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockAllSupplementsQuery : IAllSupplementsQuery
{
    private IReadOnlyCollection<SupplementData> _returnValue =
        new List<SupplementData>().AsReadOnly();

    public void SetReturnValue(IReadOnlyCollection<SupplementData>? value)
    {
        _returnValue = value ?? new List<SupplementData>().AsReadOnly();
    }

    public Task<IReadOnlyCollection<SupplementData>> Fetch()
    {
        return Task.FromResult(_returnValue);
    }
}
