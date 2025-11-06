using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record SearchUserByIdInput(
    Guid AdriaId
);

public sealed class SearchUserById : IUseCase<SearchUserByIdInput, Task<UserData>>
{
    private readonly IUserByIdQuery _userByIdQuery;
    private readonly ILogger<SearchUserById> _logger;

    public SearchUserById(IUserByIdQuery userByIdQuery, ILogger<SearchUserById> logger)
    {
        _userByIdQuery = userByIdQuery;
        _logger = logger;
    }

    public async Task<UserData> Execute(SearchUserByIdInput input)
    {
        _logger.LogInformation(
            "Fetching user with ID {UserId}",
            input.AdriaId
        );

        return (await _userByIdQuery.Fetch(input.AdriaId))
               ?? throw new ElementNotFoundException(
                   $"User with ID {input.AdriaId} not found."
               );
    }
}