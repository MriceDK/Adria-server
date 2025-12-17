using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;

public sealed record SearchSupplementsByNameInput(string Name);

public sealed class SearchSupplementsByName(
    ISupplementsByNameQuery supplementsByNameQuery,
    ILogger<SearchSupplementsByName> logger)
    : IUseCase<SearchSupplementsByNameInput, Task<IReadOnlyCollection<SupplementData>>>
{
    public async Task<IReadOnlyCollection<SupplementData>> Execute(SearchSupplementsByNameInput input)
    {
        logger.LogInformation(
            "Fetching supplements with name '{Name}'.",
            input.Name
        );

        var supplements = await supplementsByNameQuery.Fetch(input.Name);

        if (supplements.Count == 0)
        {
            throw new ElementNotFoundException("No supplements found.");
        }

        return supplements;
    }
}