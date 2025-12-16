using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Mocks;

public sealed class MockAllUsersQuery : IAllUsersQuery
{
    private IReadOnlyCollection<UserData> _returnValue =
        Array.Empty<UserData>();

    public void SetReturnValue(IReadOnlyCollection<UserData> value)
    {
        _returnValue = value;
    }

    public Task<IReadOnlyCollection<UserData>> Fetch()
    {
        return Task.FromResult(_returnValue);
    }
}