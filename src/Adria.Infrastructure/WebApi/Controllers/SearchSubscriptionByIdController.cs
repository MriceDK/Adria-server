using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using Adria.Domain.Subcriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class SearchSubscriptionByIdController
{
    public static async Task<Results<Ok<Subscription>, NotFound>> Invoke(
        [FromRoute] Guid subscriptionId,
        [FromServices] IUseCase<SearchSubscriptionByIdInput, Task<SubscriptionData>> searchSubscriptionById
    )
    {
        SubscriptionData subscriptionData = await searchSubscriptionById.Execute(
            new SearchSubscriptionByIdInput(subscriptionId)
        );

        return TypedResults.Ok(ToResponse(subscriptionData));
    }

    private static Subscription ToResponse(SubscriptionData data)
    {
        return new Subscription(
            data.Type,
            data.PricePerMonth,
            data.Advantages,
            data.SubscriptionId
        );
    }

    private SearchSubscriptionByIdController()
    {
    }
}