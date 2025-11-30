using Adria.Application.Contracts;
using Adria.Application.FoodComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers.FoodComposition;

public class CreateFoodCompositionController
{
    public static async Task<Results<Created, BadRequest<string>>> Invoke(
        [FromBody] FoodCompositionBody body,
        [FromServices] IUseCase<CreateFoodCompositionInput, Task> createFoodComposition
    )
    {
        if (body.Amount <= 0)
        {
            return TypedResults.BadRequest("Amount must be greater than zero.");
        }

        var input = new CreateFoodCompositionInput(
            body.FoodId,
            body.NutrientId,
            body.Amount
        );

        try
        {
            await createFoodComposition.Execute(input);
        }
        catch (ArgumentException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }

        return TypedResults.Created($"/api/foodcompositions/{body.FoodId}");
    }

    private CreateFoodCompositionController() { }
}

public sealed record FoodCompositionBody(
    Guid FoodId,
    Guid NutrientId,
    double Amount
);