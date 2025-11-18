using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;


public sealed record SearchSupplementsByNameInput(string Name);

public sealed class SearchSupplementsByName(
    ISupplementsByNameQuery supplementsByNameQuery,
    ILogger<SearchSupplementsByName> logger)
    : IUseCase<SearchSupplementsByNameInput, Task<IReadOnlyCollection<SupplementData?>>>
{
    public async Task<IReadOnlyCollection<SupplementData?>> Execute(SearchSupplementsByNameInput input)
    {
        logger.LogInformation(
            "Fetching supplements with name '{Name}'.",
            input.Name
        );
        
        return (await supplementsByNameQuery.Fetch(input.Name))
               ?? throw new ElementNotFoundException(
                   $"Supplement with name {input.Name} not found."
               );
    }
}