using Adria.Application.Contracts;
using Adria.Application.OrderSupplement;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Adria.Application.Contracts.Data;
using Adria.Domain.Order;

namespace Adria.Infrastructure.WebApi.Controllers.OrderSupplement;

public sealed class DeleteOrderSupplementController
{
    public static async Task<Results<Ok<OrderSupplementDetailsData>, NotFound<string>, BadRequest<string>>> Invoke(
        [FromBody] DeleteOrderSupplementDetailsInput body,
        [FromServices] IUseCase<DeleteOrderSupplementDetailsInput, Task<OrderSupplementDetails>> deleteUseCase
    )
    {
        try
        {
            var deletedEntity = await deleteUseCase.Execute(body);
            
            // Map the domain entity back to a Data contract for the response
            var result = new OrderSupplementDetailsData(
                deletedEntity.OrderId, 
                deletedEntity.SupplementId, 
                deletedEntity.Amount
            );

            return TypedResults.Ok(result);
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private DeleteOrderSupplementController() { }
}