using Adria.Application.Contracts;
using Adria.Application.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Order;

public sealed class CreateOrderController
{
    public static async Task<Results<Created<string>, BadRequest<string>>> Invoke(
        [FromBody] CreateOrderBody body,
        [FromServices] IUseCase<CreateOrderInput, Task<Guid>> createOrder
    )
    {
        if (body.AdrianId == Guid.Empty)
        {
            return TypedResults.BadRequest("AdrianId cannot be empty.");
        }
        if (body.TotalPrice <= 0)
        {
            return TypedResults.BadRequest("TotalPrice must be positive.");
        }
        
        CreateOrderInput input = new(
            OrderId: Guid.NewGuid(),
            AdrianId: body.AdrianId,
            Date: DateTime.UtcNow,
            TotalPrice: body.TotalPrice
        );

        try
        {
            Guid orderId = await createOrder.Execute(input);
            return TypedResults.Created($"/api/Order/by-id/{orderId}", orderId.ToString());
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private CreateOrderController() { }
}

public sealed record CreateOrderBody(
    Guid AdrianId,
    double TotalPrice 
);