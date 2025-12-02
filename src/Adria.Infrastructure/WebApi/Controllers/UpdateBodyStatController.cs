using Adria.Application.BodyStats;
using Adria.Application.Contracts; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class UpdateBodyStatController
{
    public static async Task<Results<NoContent, NotFound, BadRequest>> Invoke(
        [FromRoute] Guid id, 
        [FromBody] UpdateGoalBody body, 
        [FromServices] IUseCase<UpdateBodyStatGoalInput, Task> useCase
    )
    {
        var input = new UpdateBodyStatGoalInput(id, body.Goal);
        
   
        await useCase.Execute(input);

        return TypedResults.NoContent(); 
    }
}

public sealed record UpdateGoalBody(double? Goal);