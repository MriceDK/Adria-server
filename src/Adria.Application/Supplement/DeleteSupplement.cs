using Adria.Application.Contracts;
using Adria.Domain.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;

public sealed record DeleteSupplementInput(
    Guid SupplementId
);

public sealed class DeleteSupplement(ISupplementRepository supplementRepository, ILogger<CreateSupplement> logger)
    : IUseCase<DeleteSupplementInput, Task>
{
    public async Task Execute(DeleteSupplementInput input)
    {
        Domain.Order.Supplement? supplement = await supplementRepository.ById(input.SupplementId);

        if (supplement is null)
        {
            throw new ElementNotFoundException($"Supplement with ID {input.SupplementId} not found.");
        }
        
        await supplementRepository.Remove(supplement);

        logger.LogInformation(
            "Deleted order with ID {OrderId}",
            supplement.SupplementId
        );
    }
}