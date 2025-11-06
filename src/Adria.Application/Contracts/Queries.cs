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

public interface IOrderByIdQuery
{
    Task<OrderData?> Fetch(Guid orderId);
}