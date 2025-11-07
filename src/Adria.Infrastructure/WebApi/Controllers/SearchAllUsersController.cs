using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class SearchAllUsersController
{
    public static async Task<Ok<List<User>>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<UserData>>> searchAllUsers
    )
    {
        IReadOnlyCollection<UserData> userData = await searchAllUsers.Execute();

        return TypedResults.Ok(ToResponse(userData));
    }

    private static List<User> ToResponse(IReadOnlyCollection<UserData> userData)
    {
        return userData.Select(data => new User(
            Id: data.AdriaId,
            Name: data.Name,
            Job: data.Job,
            SubscriptionType: data.Subscription
        )).ToList();
    }

    private SearchAllUsersController()
    {
    }
}