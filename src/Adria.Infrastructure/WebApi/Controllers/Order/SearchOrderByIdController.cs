using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Order;

public sealed class SearchOrderByIdController
{
    public static async Task<Results<Ok<OrderData?>, NotFound<string>, BadRequest<string>>> Invoke(
        [FromRoute] Guid id,
        [FromServices] IUseCase<SearchOrderByIdInput, Task<OrderData?>> searchOrderById
    )
    {
        if (id == Guid.Empty)
        {
            return TypedResults.BadRequest("Order ID cannot be empty.");
        }
        
        SearchOrderByIdInput input = new(OrderId: id);

        try
        {
            var order = await searchOrderById.Execute(input);
            return TypedResults.Ok(order);
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private SearchOrderByIdController() { }
}