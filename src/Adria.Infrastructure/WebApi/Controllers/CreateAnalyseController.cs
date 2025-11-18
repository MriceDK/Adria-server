using Adria.Application.Analyses;
using Adria.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class CreateAnalyseController
{
    public static async Task<Results<Created, BadRequest<string>>> Invoke(
        [FromBody] CreateAnalyseBody body,
        [FromServices] IUseCase<CreateAnalyseInput, Task<Guid>> createAnalyse
    )
    {
        if (body.Details is null || body.Details.Count == 0)
        {
            return TypedResults.BadRequest("Details cannot be empty.");
        }

        var detailsInput = body.Details
            .Select(d => new AnalyseDetailInput(d.BodyStatId, d.Value))
            .ToList();

        var input = new CreateAnalyseInput(
            AdrianId: body.UserId,
            DateTime: DateTime.UtcNow, 
            Details: detailsInput
        );

        Guid analyseId = await createAnalyse.Execute(input);
        
        return TypedResults.Created($"/api/analyses/{analyseId}");
    }
}

public sealed record CreateAnalyseBody(
    Guid UserId,
    List<AnalyseDetailBody> Details
);

public sealed record AnalyseDetailBody(
    Guid BodyStatId,
    double Value
);