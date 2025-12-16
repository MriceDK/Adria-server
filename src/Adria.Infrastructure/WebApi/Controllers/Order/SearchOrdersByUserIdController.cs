using Adria.Application.Contracts;
using Adria.Application.Order;
using Adria.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace Adria.Infrastructure.WebApi.Controllers.Order;

public sealed class SearchOrderByUserIdController
{
    public static async Task<
        Results<
            Ok<IReadOnlyCollection<OrderWithSupplementsData>>,
            NotFound<string>,
            BadRequest<string>
        >
    > Invoke(
        [FromRoute] Guid adrianId,
        [FromServices]
        IUseCase<
            SearchOrderByUserIdInput,
            Task<IReadOnlyCollection<OrderWithSupplementsData>>
        > searchOrderByUserId
    )
    {
        if (adrianId == Guid.Empty)
        {
            return TypedResults.BadRequest("AdrianId cannot be empty.");
        }

        var input = new SearchOrderByUserIdInput(adrianId);

        try
        {
            var orders = await searchOrderByUserId.Execute(input);
            return TypedResults.Ok(orders);
        }
        catch (ElementNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
    }

    private SearchOrderByUserIdController() { }
}