using Adria.Application.Contracts.Data;
using Adria.Application.Scanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.Scanner;

public static class ScanFoodController
{
    public static async Task<Results<Ok<ScannedFoodResult>, BadRequest>> Invoke(
        [FromRoute] Guid adrianId,
        [FromServices] ScanFood scanFood
    )
    {
        if (adrianId == Guid.Empty)
            return TypedResults.BadRequest();

        var result = await scanFood.Execute(new ScanFoodInput(adrianId));

        return TypedResults.Ok(result);
    }
}
