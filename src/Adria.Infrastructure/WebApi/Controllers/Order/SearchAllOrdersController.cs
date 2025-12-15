using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.Order;

public sealed class SearchAllOrdersController
{
    public static async Task<Results<Ok<IReadOnlyCollection<OrderData>>, NotFound<string>>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<OrderData>>> searchAllOrders
    )
    {
        try
        {
            var orders = await searchAllOrders.Execute();
            return TypedResults.Ok(orders);
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private SearchAllOrdersController() { }
}