using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Order;

public sealed class SearchOrderByUserIdController
{
    public static async Task<Results<Ok<IReadOnlyCollection<OrderData>>, NotFound<string>, BadRequest<string>>> Invoke(
        [FromRoute] Guid adrianId,
        [FromServices] IUseCase<SearchOrderByUserIdInput, Task<IReadOnlyCollection<OrderData>>> searchOrderByUserId
    )
    {
        if (adrianId == Guid.Empty)
        {
            return TypedResults.BadRequest("AdrianId cannot be empty.");
        }
        
        SearchOrderByUserIdInput input = new(AdrianId: adrianId);

        try
        {
            var orders = await searchOrderByUserId.Execute(input);
            return TypedResults.Ok(orders);
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private SearchOrderByUserIdController() { }
}