using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Supplement;

namespace Adria.Application.OrderSupplement;

public sealed class CalculateOrderTotalPrice(
    IUseCase<SearchSupplementsByIdInput, Task<SupplementData?>> getSupplementById
) : IUseCase<IReadOnlyCollection<CreateOrderSupplementDetailItem>, Task<double>>
{
    public async Task<double> Execute(IReadOnlyCollection<CreateOrderSupplementDetailItem> items)
    {
        double total = 0;

        foreach (var item in items)
        {
            var supplement = await getSupplementById.Execute(
                new SearchSupplementsByIdInput(item.SupplementId)
            ) ?? throw new InvalidOperationException(
                $"Supplement {item.SupplementId} not found."
            );

            total += supplement.Price * item.Amount;
        }

        return total;
    }
}