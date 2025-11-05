using Adria.Application.Contracts.Data;

namespace Adria.Application.Contracts;

public interface ISubscriptionByIdQuery
{
    Task<SubscriptionData?> Fetch(Guid subscriptionId);
}