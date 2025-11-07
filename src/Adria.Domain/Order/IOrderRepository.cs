namespace Adria.Domain.Order;

public interface IOrderRepository
{
    Task<Order> ByOrderId(Guid orderId);
    Task<IReadOnlyCollection<Order>> ByUserId(Guid adrianId);
    Task<IReadOnlyCollection<Order>> GetAll();
    Task Save(Order order);
    Task Remove(Order order);
}