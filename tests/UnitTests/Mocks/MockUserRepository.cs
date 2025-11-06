using Adria.Domain.Users;

namespace UnitTests.Mocks;

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = [];

    public Task Save(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User?> ById(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.AdriaId == id);
        return Task.FromResult(user);
    }
}