namespace Adria.Domain.Order;

public interface IOrderSupplementDetailsRepository
{
    Task<OrderSupplementDetails?> ByOrderId(Guid orderId);
    Task<IReadOnlyCollection<OrderSupplementDetails>> BySupplementId(Guid supplementId);
    Task<IReadOnlyCollection<OrderSupplementDetails>> GetAll();
    Task Save(OrderSupplementDetails orderSupplementDetails);
    Task Remove(OrderSupplementDetails orderSupplementDetails);
}