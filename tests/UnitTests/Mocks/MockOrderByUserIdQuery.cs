using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
namespace UnitTests.Mocks;

public class MockOrderByUserIdQuery : IOrderByUserIdQuery
{
    private readonly Dictionary<Guid, IReadOnlyCollection<OrderData>> _dataStore = new();

    public void SetOrders(Guid adriaId, IList<OrderData> ordersData)
    {
        _dataStore[adriaId] = (IReadOnlyCollection<OrderData>)ordersData;

    }
    
    public Task<IReadOnlyCollection<OrderData>?> Fetch(Guid userId)
    {
        _dataStore.TryGetValue(userId, out var data);
        return Task.FromResult(data);
    }
}