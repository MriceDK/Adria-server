namespace Adria.Domain.Users;

public interface IUserRepository
{
    Task<User?> ById(Guid userId);

    Task Save(User user);
}