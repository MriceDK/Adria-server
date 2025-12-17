using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;

public sealed record SearchSupplementsByTypeInput(string Type);

public sealed class SearchSupplementsByType(
    ISupplementsByTypeQuery supplementsByTypeQuery,
    ILogger<SearchSupplementsByType> logger)
    : IUseCase<SearchSupplementsByTypeInput, Task<IReadOnlyCollection<SupplementData>>>
{
    public async Task<IReadOnlyCollection<SupplementData>> Execute(SearchSupplementsByTypeInput input)
    {
        logger.LogInformation(
            "Fetching supplements with type '{Type}'.",
            input.Type
        );

        var supplements = await supplementsByTypeQuery.Fetch(input.Type);

        if (supplements.Count == 0)
        {
            throw new ElementNotFoundException("No supplements found.");
        }

        return supplements;
    }
}
