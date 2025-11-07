using Adria.Application.Contracts.Data;

namespace Adria.Application.Contracts;

public interface ISubscriptionByIdQuery
{
    Task<SubscriptionData?> Fetch(Guid subscriptionId);
}

public interface IUserByIdQuery
{
    Task<UserData?> Fetch(Guid userId);
}

public interface IAllSubscriptionsQuery
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}

public interface IAllUsersQuery
{
    Task<IReadOnlyCollection<UserData>> Fetch();
}