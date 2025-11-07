using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain.Subcriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class CreateUserController
{
    public static async Task<Results<Created, BadRequest<string>>> Invoke(
        [FromBody] CreateUserBody body,
        [FromServices] IUseCase<CreateUserInput, Task<Guid>> createUser,
        [FromServices] ISubscriptionRepository subscriptionRepository 
    )
    {
  
        if (string.IsNullOrWhiteSpace(body.Name) || string.IsNullOrWhiteSpace(body.Job))
        {
            return TypedResults.BadRequest("Name or Job cannot be empty.");
        }


        var subscription = await subscriptionRepository.ById(body.SubscriptionId);
        if (subscription is null)
        {
            return TypedResults.BadRequest($"Subscription with ID {body.SubscriptionId} not found.");
        }

        CreateUserInput input = new(
            Name: body.Name,
            Job: body.Job,
            Subscription: subscription 
        );

        Guid userId = await createUser.Execute(input);
        
        return TypedResults.Created($"/api/users/{userId}");
    }

    private CreateUserController() { }
}

public sealed record CreateUserBody(
    string Name,
    string Job,
    Guid SubscriptionId 
);