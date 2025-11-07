using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public class SearchAllUsers : IUseCase<Task<IReadOnlyCollection<UserData>>>
{
    private readonly IAllUsersQuery _allUsersQuery;
    private readonly ILogger<SearchAllUsers> _logger;

    public SearchAllUsers(
        IAllUsersQuery allUsersQuery,
        ILogger<SearchAllUsers> logger
    )
    {
        _allUsersQuery = allUsersQuery;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<UserData>> Execute()
    {
        _logger.LogInformation(
            "Fetching all users"
        );
        return await _allUsersQuery.Fetch();
    }
}