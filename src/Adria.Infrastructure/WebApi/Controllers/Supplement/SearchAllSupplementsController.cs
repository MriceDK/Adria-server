using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Supplement;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Supplement;

public sealed class SearchAllSupplementsController
{
    public static async Task<Results<Ok<IReadOnlyCollection<SupplementData?>>, NotFound<string>>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<SupplementData?>>> searchAllSupplements
    )
    {
        try
        {
            var supplements = await searchAllSupplements.Execute();
            
            return TypedResults.Ok(supplements);
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private SearchAllSupplementsController() { }
}