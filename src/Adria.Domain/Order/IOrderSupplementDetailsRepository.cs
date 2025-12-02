namespace Adria.Domain.Order;

public interface IOrderSupplementDetailsRepository
{
    Task<IReadOnlyCollection<OrderSupplementDetails>> ByOrderId(Guid orderId);
    Task<IReadOnlyCollection<OrderSupplementDetails>> BySupplementId(Guid supplementId);
    Task<OrderSupplementDetails> ByOrderAndSupplementId(Guid orderId, Guid supplementId);
    Task Save(OrderSupplementDetails orderSupplementDetails);
    Task Remove(OrderSupplementDetails orderSupplementDetails);
}