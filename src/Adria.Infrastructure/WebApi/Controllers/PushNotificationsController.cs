using Adria.Application.Contracts;
using Adria.Application.PushNotifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class PushNotificationsController
{
    public static async Task<Results<Ok, BadRequest>> Subscribe(
        [FromBody] SubscribeBody body,
        [FromServices] IUseCase<SubscribeToPushInput, Task> useCase
    )
    {
        var input = new SubscribeToPushInput(
            body.UserId,
            body.Subscription.Endpoint,
            new PushSubscriptionKeys(
                body.Subscription.Keys.P256dh, 
                body.Subscription.Keys.Auth
            )
        );

        await useCase.Execute(input);

        return TypedResults.Ok();
    }
}

public record SubscribeBody(
    Guid UserId, 
    BrowserSubscription Subscription 
);

public record BrowserSubscription(
    string Endpoint,
    BrowserKeys Keys
);

public record BrowserKeys(
    string P256dh,
    string Auth
);