using Adria.Application.Contracts;
using Adria.Application.Subscriptions;
using Adria.Domain.Subcriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class CreateSubscriptionController
{
    public static async Task<Results<Created, BadRequest<string>>> Invoke(
        [FromBody] CreateSubscriptionBody body,
        [FromServices] IUseCase<CreateSubscriptionInput, Task<Guid>> createSubscription
    )
    {
        if (!Enum.TryParse(typeof(SubscriptionType), body.SubscriptionType, ignoreCase: true, out var parsedType))
        {
            return TypedResults.BadRequest("Invalid SubscriptionType.");
        }

        if (string.IsNullOrWhiteSpace(body.Advantages) || body.PricePerMonth < 0)
        {
            return TypedResults.BadRequest("Advantages cannot be empty and PricePerMonth must be zero or positive.");
        }

        CreateSubscriptionInput input = new(
            (SubscriptionType)parsedType,
            body.PricePerMonth,
            body.Advantages
        );

        Guid subscriptionId = await createSubscription.Execute(input);

        return TypedResults.Created($"/api/subscriptions/{subscriptionId}");
    }

    private CreateSubscriptionController()
    {
    }
}

public sealed record CreateSubscriptionBody(
    string SubscriptionType,
    double PricePerMonth,
    string Advantages
);