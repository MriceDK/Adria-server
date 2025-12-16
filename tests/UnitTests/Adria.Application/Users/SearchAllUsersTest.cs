using Adria.Application.Users;
using Adria.Application.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Adria.Application.Users;

public sealed class SearchAllUsersTest
{
    [Fact]
    public async Task Execute_ReturnsAllUsers()
    {
        var query = new MockAllUsersQuery();
        var logger = new MockLogger<SearchAllUsers>();

        var users = new[]
        {
            new UserData(Guid.NewGuid(), "Alice", "adventurer","premium"),
            new UserData(Guid.NewGuid(), "Bob", "teacher", "free"),
        };

        query.SetReturnValue(users);

        var useCase = new SearchAllUsers(query, logger);

        var result = await useCase.Execute();

        Assert.Equal(2, result.Count);
        Assert.Equal(users, result);
        Assert.Single(logger.LoggedMessages);
        Assert.Contains("Fetching all users", logger.LoggedMessages[0]);
    }
}