using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class OrderSupplementsByOrderQuery(
    IOrderSupplementDetailsRepository repository) : IOrderSupplementsByOrderQuery
{
    public async Task<IReadOnlyCollection<OrderSupplementDetailsData>> Fetch(Guid orderId)
    {
        var entities = await repository.ByOrderId(orderId);
        return entities.Select(e => new OrderSupplementDetailsData(
            e.OrderId, 
            e.SupplementId, 
            e.Amount)).ToList().AsReadOnly();
    }
}