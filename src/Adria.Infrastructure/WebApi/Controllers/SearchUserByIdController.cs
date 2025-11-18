using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class SearchUserByIdController
{
    public static async Task<Results<Ok<User>, NotFound>> Invoke(
        [FromRoute] Guid userId,
        [FromServices] IUseCase<SearchUserByIdInput, Task<UserData>> searchUserById
    )
    {
        UserData userData = await searchUserById.Execute(new SearchUserByIdInput(userId));

        return TypedResults.Ok(ToResponse(userData));
    }

    private static User ToResponse(UserData userData)
    {
        return new User(
            Id: userData.AdriaId,
            Name: userData.Name,
            Job: userData.Job,
            SubscriptionType: userData.Subscription
        );
    }

    private SearchUserByIdController()
    {
    }
}