namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed record User(
    Guid Id,
    string Name,
    string Job,
    string SubscriptionType
);