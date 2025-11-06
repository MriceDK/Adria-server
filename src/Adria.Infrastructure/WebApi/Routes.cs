using Adria.Infrastructure.WebApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Adria.Infrastructure.WebApi;

public static class Routes
{
    private readonly static string APPLICATION_JSON = "application/json";

    public static OpenApiInfo OpenApiInfo { get; } = new OpenApiInfo
    {
        Version = "v1",
        Title = "Nutriscan Web Api",
        Description = "A simple API to Nutriscan Resources",
        Contact = new OpenApiContact
        {
            Name = "Group 08",
            Email = "info@group-08.adria"
        }
    };

    public static WebApplication MapRoutes(this WebApplication app)
    {
        MapSubscriptionRoutes(app);
        return app;
    }

    private static void MapSubscriptionRoutes(WebApplication app)
    {
        var subscriptionRoutes = app.MapGroup("/api/subscriptions")
            .WithTags("Subscriptions")
            .WithDescription("All endpoints related to Taskly subscriptions.")
            .WithOpenApi();

        subscriptionRoutes
            .MapPost("/", CreateSubscriptionController.Invoke)
            .WithDescription("Create a new todo list.")
            .WithName(nameof(CreateSubscriptionController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        subscriptionRoutes
            .MapGet("/", SearchAllSubscriptionsController.Invoke)
            .WithDescription("Get all users .")
            .WithName(nameof(SearchAllSubscriptionsController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        subscriptionRoutes
            .MapPost("/", CreateSubscriptionController.Invoke)
            .WithDescription("Create a new user.")
            .WithName(nameof(CreateSubscriptionController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }
}