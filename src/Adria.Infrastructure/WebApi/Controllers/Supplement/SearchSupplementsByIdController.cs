using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Supplement;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Supplement;

public sealed class SearchSupplementsByIdController
{
    public static async Task<Results<Ok<SupplementData?>, NotFound<string>, BadRequest<string>>> Invoke(
        [FromRoute] Guid id,
        [FromServices] IUseCase<SearchSupplementsByIdInput, Task<SupplementData?>> searchSupplementsById
    )
    {
        if (id == Guid.Empty)
        {
            return TypedResults.BadRequest("Supplement ID cannot be empty.");
        }
        
        SearchSupplementsByIdInput input = new(Id: id);

        try
        {
            var supplement = await searchSupplementsById.Execute(input);
            
            return TypedResults.Ok(supplement);
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private SearchSupplementsByIdController() { }
}