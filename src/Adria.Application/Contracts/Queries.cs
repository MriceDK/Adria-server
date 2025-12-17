using Adria.Application.Contracts.Data;
using Adria.Domain.BodyStats;

namespace Adria.Application.Contracts;

public interface ISubscriptionByIdQuery
{
    Task<SubscriptionData?> Fetch(Guid subscriptionId);
}

public interface IUserByIdQuery
{
    Task<UserData?> Fetch(Guid userId);
}

public interface IBodyStatsQuery
{
    Task<IReadOnlyCollection<BodyStatData>?> Fetch(Guid userId);
}

public interface IAllSubscriptionsQuery
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}

public interface IAllUsersQuery
{
    Task<IReadOnlyCollection<UserData>> Fetch();
}

public interface IBodyStatRepository
{
    Task<BodyStat?> ById(string id); 
    Task Update(BodyStat bodyStat);
}

public interface ISupplementsByNameQuery
{
    Task<IReadOnlyCollection<SupplementData>> Fetch(string name);
}

public interface ISupplementsByTypeQuery
{
    Task<IReadOnlyCollection<SupplementData>> Fetch(string type);
}

public interface ISupplementsByIdQuery
{
    Task<SupplementData?> Fetch(Guid supplementId);
}

public interface IAllSupplementsQuery
{
    Task<IReadOnlyCollection<SupplementData>> Fetch();
}

public interface IOrderByIdQuery
{
    Task<OrderData?> Fetch(Guid id);
}

public interface IOrderByUserIdQuery
{
    Task<IReadOnlyCollection<OrderData?>> Fetch(Guid id);
}

public interface IOrderSupplementsByOrderQuery
{
    Task<IReadOnlyCollection<OrderSupplementDetailsData>> Fetch(Guid orderId);
}

public interface IOrderSupplementsBySupplementQuery
{
    Task<IReadOnlyCollection<OrderSupplementDetailsData>> Fetch(Guid supplementId);
}