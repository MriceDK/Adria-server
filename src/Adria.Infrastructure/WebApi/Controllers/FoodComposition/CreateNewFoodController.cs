using System;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed record CreateFoodBody(
    string Name,
    string Type,
    bool Edible
);

public sealed record CreateFoodResponse(Guid FoodId);

public class CreateNewFoodController
{
    public static async Task<Results<Created<CreateFoodResponse>, BadRequest<string>>> Invoke(
        [FromBody] CreateFoodBody body,
        [FromServices] IUseCase<FoodData, Task<Guid>> createFood
    )
    {
        if (string.IsNullOrWhiteSpace(body.Name) || string.IsNullOrWhiteSpace(body.Type))
        {
            return TypedResults.BadRequest("Name or Type cannot be empty.");
        }

        var input = new FoodData(
            FoodId: Guid.NewGuid(),
            Name: body.Name,
            Type: body.Type,
            Edible: body.Edible
        );

        Guid foodId = await createFood.Execute(input);

        return TypedResults.Created(
            $"/api/foodcompositions/{foodId}",
            new CreateFoodResponse(foodId)
        );
    }

    private CreateNewFoodController() { }
}