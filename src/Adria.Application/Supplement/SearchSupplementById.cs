using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;


public sealed record SearchSupplementsByIdInput(Guid Id);

public sealed class SearchSupplementsById(
    ISupplementsByIdQuery supplementsByIdQuery,
    ILogger<SearchSupplementsById> logger)
    : IUseCase<SearchSupplementsByIdInput, Task<SupplementData?>>
{
    public async Task<SupplementData?> Execute(SearchSupplementsByIdInput input)
    {
        logger.LogInformation(
            "Fetching supplements with id '{Id}'.",
            input.Id
        );
        
        return (await supplementsByIdQuery.Fetch(input.Id))
               ?? throw new ElementNotFoundException(
                   $"Supplement with id {input.Id} not found."
               );
    }
}