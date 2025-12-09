using Adria.Application.Contracts.Data;
using Adria.Application.FoodComposition;
using Adria.Domain.Food;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.FoodComposition;

public sealed class GetAllFoodsController
{
    public static async Task<Results<Ok<IReadOnlyCollection<FoodData>>, NotFound>> Invoke(
        [FromServices] GetAllFoods getAllFoods
    )
    {
        var data = await getAllFoods.Execute();

        if (data.Count == 0)
            return TypedResults.NotFound();

        return TypedResults.Ok(data);
    }

    private GetAllFoodsController() { }
}