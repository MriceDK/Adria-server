using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.FoodComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.FoodComposition;

public sealed class GetFoodCompositionsByNutrientController
{
    public static async Task<Results<Ok<IReadOnlyCollection<Domain.Food.FoodComposition>>, NotFound>> Invoke(
        [FromRoute] string Type,
        [FromServices] IUseCase<SearchFoodCompositionsByNutrientIdInput, Task<IReadOnlyCollection<FoodCompositionData>>> searchFoodCompositions
    )
    {
        if (string.IsNullOrWhiteSpace(Type))
        {
            return TypedResults.NotFound();
        }

        var input = new SearchFoodCompositionsByNutrientIdInput(Type);

        IReadOnlyCollection<FoodCompositionData> result = await searchFoodCompositions.Execute(input);

        if (result == null || result.Count == 0)
        {
            return TypedResults.NotFound();
        }

        var response = ToResponse(result);
        return TypedResults.Ok(response);
    }

    private static IReadOnlyCollection<Domain.Food.FoodComposition> ToResponse(
        IReadOnlyCollection<FoodCompositionData> foodCompositionData
    )
    {

        return foodCompositionData
            .Select(data => new Domain.Food.FoodComposition(
                data.FoodId,
                data.NutrientId,
                data.Amount
            ))
            .ToList();
    }

    private GetFoodCompositionsByNutrientController()
    {
    }
}