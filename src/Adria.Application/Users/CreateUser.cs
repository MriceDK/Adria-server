using Adria.Application.Contracts;
using Adria.Domain.Subcriptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record CreateUserInput(
    string Name,
    string Job,
    Subscription Subscription
);

public sealed class CreateUser : IUseCase<CreateUserInput, Task<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateUser> _logger;

    public CreateUser(IUserRepository userRepository, ILogger<CreateUser> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<Guid> Execute(CreateUserInput input)
    {
        User user = new(input.Name, input.Job, input.Subscription);

        await _userRepository.Save(user);

        _logger.LogInformation(
            "New user created with ID {UserId}, Name: {Name}, Subscription: {Subscription}",
            user.AdriaId,
            input.Name,
            input.Subscription
        );

        return user.AdriaId;
    }
}