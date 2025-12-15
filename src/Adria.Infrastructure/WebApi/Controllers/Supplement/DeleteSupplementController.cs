using Adria.Application.Contracts;
using Adria.Application.Supplement;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Supplement;

public sealed class DeleteSupplementController
{
    public static async Task<Results<NoContent, NotFound<string>, BadRequest<string>>> Invoke(
        [FromBody] DeleteSupplementBody body,
        [FromServices] IUseCase<DeleteSupplementInput, Task> deleteSupplement
    )
    {
        if (body.SupplementId == Guid.Empty)
        {
            return TypedResults.BadRequest("SupplementId cannot be empty.");
        }
        
        DeleteSupplementInput input = new(SupplementId: body.SupplementId);

        try
        {
            await deleteSupplement.Execute(input);
            return TypedResults.NoContent();
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private DeleteSupplementController() { }
}

public sealed record DeleteSupplementBody(
    Guid SupplementId
);