using Adria.Application.Contracts;
using Adria.Application.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Order;

public sealed class DeleteOrderController
{
    public static async Task<Results<NoContent, NotFound<string>, BadRequest<string>>> Invoke(
        [FromBody] DeleteOrderBody body,
        [FromServices] IUseCase<DeleteOrderInput, Task> deleteOrder
    )
    {
        if (body.OrderId == Guid.Empty)
        {
            return TypedResults.BadRequest("OrderId cannot be empty.");
        }
        
        DeleteOrderInput input = new(OrderId: body.OrderId);

        try
        {
            await deleteOrder.Execute(input);
            return TypedResults.NoContent();
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private DeleteOrderController() { }
}

public sealed record DeleteOrderBody(Guid OrderId);