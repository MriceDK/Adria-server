using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;

public sealed record SearchSupplementsByTypeInput(string Type);

public sealed class SearchSupplementsByType(
    ISupplementsByTypeQuery supplementsByTypeQuery,
    ILogger<SearchSupplementsByType> logger)
    : IUseCase<SearchSupplementsByTypeInput, Task<IReadOnlyCollection<SupplementData?>>>
{
    public async Task<IReadOnlyCollection<SupplementData?>> Execute(SearchSupplementsByTypeInput input)
    {
        logger.LogInformation(
            "Fetching supplements with type '{Type}'.",
            input.Type
        );
        
        return (await supplementsByTypeQuery.Fetch(input.Type))
               ?? throw new ElementNotFoundException(
                   $"Supplement with type {input.Type} not found."
               );
    }
}