using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.OrderSupplement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.OrderSupplement;

public sealed class SearchOrderSupplementsByOrderController
{
    public static async Task<Ok<IReadOnlyCollection<OrderSupplementDetailsData>>> Invoke(
        [FromRoute] Guid orderId,
        [FromServices] IUseCase<SearchOrderSupplementsByOrderIdInput, Task<IReadOnlyCollection<OrderSupplementDetailsData>>> searchUseCase
    )
    {
        var result = await searchUseCase.Execute(new SearchOrderSupplementsByOrderIdInput(orderId));
        return TypedResults.Ok(result);
    }

    private SearchOrderSupplementsByOrderController() { }
}