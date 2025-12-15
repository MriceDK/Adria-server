using Adria.Application.Contracts;
using Adria.Application.Supplement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Adria.Infrastructure.WebApi.Controllers.Supplement;

public sealed class CreateSupplementController
{
    public static async Task<Results<Created<string>, BadRequest<string>>> Invoke(
        [FromBody] CreateSupplementBody body,
        [FromServices] IUseCase<CreateSupplementInput, Task<Guid>> createSupplement
    )
    {
        if (string.IsNullOrWhiteSpace(body.Name) || string.IsNullOrWhiteSpace(body.Type))
        {
            return TypedResults.BadRequest("Name and Type cannot be empty.");
        }
        if (body.Price <= 0 || body.Stock < 0)
        {
            return TypedResults.BadRequest("Price must be positive and Stock cannot be negative.");
        }
        
        CreateSupplementInput input = new(
            SupplementId: Guid.NewGuid(),
            Name: body.Name,
            Type: body.Type,
            Price: body.Price,
            Stock: body.Stock
        );

        try
        {
            Guid supplementId = await createSupplement.Execute(input);
            return TypedResults.Created($"/api/Supplement/by-id/{supplementId}", supplementId.ToString());
        }
        catch (Exception ex) when (ex is ArgumentException)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private CreateSupplementController() { }
}

public sealed record CreateSupplementBody(
    string Name,
    string Type,
    double Price,
    int Stock 
);