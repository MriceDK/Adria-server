using Adria.Application.Contracts;
using Adria.Application.Order;
using Adria.Application.OrderSupplement;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using CreateOrderInput = Adria.Application.OrderSupplement.CreateOrderInput;

namespace Adria.Infrastructure.WebApi.Controllers.OrderSupplement;

public sealed class CreateOrderSupplementController
{
    public static async Task<Results<Ok<CreateOrderSupplementResponse>, BadRequest<string>>> Invoke(
        [FromBody] CreateOrderSupplementBody body,
        [FromServices] IUseCase<CreateOrderInput, Task<Guid>> useCase
    )
    {
        if (body.Supplements is null || body.Supplements.Count == 0)
            return TypedResults.BadRequest("At least one supplement is required.");

        if (body.Supplements.Any(s => s.Amount <= 0))
            return TypedResults.BadRequest("All amounts must be greater than 0.");

        var items = body.Supplements
            .Select(s => new CreateOrderSupplementDetailItem(
                s.SupplementId,
                s.Amount
            ))
            .ToList();

        var orderId = await useCase.Execute(
            new CreateOrderInput(body.AdrianId, items)
        );

        return TypedResults.Ok(new CreateOrderSupplementResponse(orderId));
    }
}
public sealed record CreateOrderSupplementBody(
    Guid AdrianId,
    IReadOnlyCollection<CreateOrderSupplementItemBody> Supplements
);

public sealed record CreateOrderSupplementItemBody(
    Guid SupplementId,
    int Amount
);