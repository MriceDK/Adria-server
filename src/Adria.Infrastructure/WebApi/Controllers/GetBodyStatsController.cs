using Adria.Application.BodyStats;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class GetBodyStatsController
{
    public static async Task<Ok<IReadOnlyCollection<BodyStatData>>> Invoke(
        [FromRoute] Guid userId,
        [FromServices] IUseCase<GetLatestBodyStatsInput, Task<IReadOnlyCollection<BodyStatData>>> getStats
    )
    {
        var result = await getStats.Execute(new GetLatestBodyStatsInput(userId));
        return TypedResults.Ok(result);
    }
}