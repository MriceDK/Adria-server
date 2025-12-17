using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.FoodComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.FoodComposition;

public static class GetFoodCompositionsByFoodController
{
    public static async Task<Results<Ok<List<Domain.Food.FoodComposition>>, NotFound>
    > Invoke(
        [FromRoute] string foodName,
        [FromServices] IUseCase<
            SearchFoodCompositionsByFoodIdInput,
            Task<IReadOnlyCollection<FoodCompositionData>>
        > searchFoodCompositions
    )
    {
        if (string.IsNullOrWhiteSpace(foodName))
        {
            return TypedResults.NotFound();
        }

        var input = new SearchFoodCompositionsByFoodIdInput(foodName);
        IReadOnlyCollection<FoodCompositionData> result =
            await searchFoodCompositions.Execute(input);

        if (result.Count == 0)
        {
            return TypedResults.NotFound();
        }

        var response = ToResponse(result);
        return TypedResults.Ok(response);
    }

    private static List<Domain.Food.FoodComposition> ToResponse(
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
}