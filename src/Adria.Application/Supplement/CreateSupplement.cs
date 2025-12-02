using Adria.Application.Contracts;
using Adria.Domain.Order;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;

public sealed record CreateSupplementInput(
    Guid SupplementId,
    string Name,
    string Type,
    double Price,
    int Stock
);

public sealed class CreateSupplement(ISupplementRepository supplementRepository, ILogger<CreateSupplement> logger)
    : IUseCase<CreateSupplementInput, Task<Guid>>
{
    public async Task<Guid> Execute(CreateSupplementInput input)
    {
        Domain.Order.Supplement supplement = new(input.SupplementId, input.Name, input.Type, input.Price, input.Stock);

        await supplementRepository.Save(supplement);

        logger.LogInformation(
            "New supplement created with ID {SupplementId}, Name: {Name}, Type: {Type}, Price: {Price}, Stock: {Stock}.",
            supplement.SupplementId,
            supplement.Name,
            supplement.Type,
            supplement.Price,
            supplement.Stock
        );

        return supplement.SupplementId;
    }
}