using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Supplement;

public sealed class SearchAllSupplements(
    IAllSupplementsQuery allSupplementsQuery,
    ILogger<SearchAllSupplements> logger)
    : IUseCase<Task<IReadOnlyCollection<SupplementData?>>>
{
    public async Task<IReadOnlyCollection<SupplementData?>> Execute()
    {
        logger.LogInformation(
            "Fetching all supplements..."
        );
        
        return (await allSupplementsQuery.Fetch())
               ?? throw new ElementNotFoundException(
                   $"No supplements found."
               );
    }
}