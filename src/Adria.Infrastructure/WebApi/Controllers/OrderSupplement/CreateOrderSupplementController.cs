using Adria.Application.Contracts;
using Adria.Application.OrderSupplement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.OrderSupplement;

public sealed class CreateOrderSupplementController
{
    public static async Task<Results<Ok, BadRequest<string>>> Invoke(
        [FromBody] AddSupplementBody body,
        [FromServices] IUseCase<CreateOrderSupplementDetailsInput, Task> useCase
    )
    {
        if (body.Amount <= 0) return TypedResults.BadRequest("Amount must be greater than 0.");

        await useCase.Execute(new CreateOrderSupplementDetailsInput(
            body.OrderId, 
            body.SupplementId, 
            body.Amount));

        return TypedResults.Ok();
    }
}

public sealed record AddSupplementBody(Guid OrderId, Guid SupplementId, int Amount);