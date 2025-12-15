using Adria.Application.Scanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.Scanner;

public static class RemoveScanFoodController
{
    public static async Task<Results<NoContent, BadRequest>> Invoke(
        [FromRoute] Guid scanId,
        [FromServices] RemoveScanFood removeScanFood
    )
    {
        if (scanId == Guid.Empty)
            return TypedResults.BadRequest();

        await removeScanFood.Execute(new RemoveScanFoodInput(scanId));

        return TypedResults.NoContent();
    }
}