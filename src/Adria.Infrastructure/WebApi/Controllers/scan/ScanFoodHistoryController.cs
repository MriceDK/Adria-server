using Adria.Application.Contracts.Data;
using Adria.Application.Scanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.Scanner;

public static class ScanFoodHistoryController
{
    public static async Task<Results<Ok<IReadOnlyCollection<ScannedFoodResult>>, BadRequest>> Invoke(
        [FromRoute] Guid adrianId,
        [FromServices] GetScanHistory useCase
    )
    {
        if (adrianId == Guid.Empty)
            return TypedResults.BadRequest();

        var result = await useCase.Execute(new GetScanHistoryInput(adrianId));

        return TypedResults.Ok(result);
    }
}