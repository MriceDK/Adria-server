using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Subcriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class SearchAllSubscriptionsController
{
    public static async Task<Results<Ok<List<Subscription>>, NotFound>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<SubscriptionData>>> searchAllSubscriptions
    )
    {
        IReadOnlyCollection<SubscriptionData> subscriptionData = await searchAllSubscriptions.Execute();

        return TypedResults.Ok(ToResponse(subscriptionData));
    }

    private static List<Subscription> ToResponse(IReadOnlyCollection<SubscriptionData> subscriptionData)
    {
        return subscriptionData.Select(data => new Subscription(
            data.Type,
            data.PricePerMonth,
            data.Advantages,
            data.SubscriptionId
        )).ToList();
    }

    private SearchAllSubscriptionsController()
    {
    }
}