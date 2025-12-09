using Adria.Infrastructure.WebApi.Controllers;
using Adria.Infrastructure.WebApi.Controllers.FoodComposition;
using Adria.Infrastructure.WebApi.Controllers.Scanner;
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
        MapUserRoutes(app);
        MapFoodCompositionRoutes(app);
        MapAnalyseRoutes(app);
        MapScannerRoutes(app);
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
    }

    private static void MapFoodCompositionRoutes(WebApplication app)
    {
        var foodCompositionRoutes = app.MapGroup("/api/FoodComposition")
            .WithTags("FoodComposition")
            .WithDescription("All endpoints related to Food Composition.")
            .WithOpenApi();
        foodCompositionRoutes
            .MapPost("/create", CreateFoodCompositionController.Invoke)
            .WithDescription("Create a new food composition.")
            .WithName(nameof(CreateFoodCompositionController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        foodCompositionRoutes
            .MapGet("/by-food/{foodName}", GetFoodCompositionsByFoodController.Invoke)
            .WithDescription("Get all foods by name.")
            .WithName(nameof(GetFoodCompositionsByFoodController))
            .WithOpenApi();

        foodCompositionRoutes
            .MapGet("/by-nutrient/{type}", GetFoodCompositionsByNutrientController.Invoke)
            .WithDescription("Get all foods by nutrient.")
            .WithName(nameof(GetFoodCompositionsByNutrientController))
            .WithOpenApi();
        foodCompositionRoutes
            .MapPost("/food/create", CreateNewFoodController.Invoke)
            .WithDescription("Create a new food.")
            .WithName(nameof(CreateNewFoodController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        foodCompositionRoutes
            .MapGet("/food/allfood", GetAllFoodsController.Invoke)
            .WithDescription("Get all foods")
            .WithName(nameof(GetAllFoodsController))
            .WithOpenApi();

    }

    private static void MapScannerRoutes(WebApplication app)
    {
        var ScannerRoutes = app.MapGroup("/api/Scanner")
            .WithTags("Scanner")
            .WithDescription("All endpoints related to scanner.")
            .WithOpenApi();
        ScannerRoutes
            .MapPost("/ScanFood/{adrianId}", ScanFoodController.Invoke)
            .WithOpenApi()
            .WithDescription("Scan a random food for a user.");
        ScannerRoutes
            .MapGet("/history/{adrianId}", ScanFoodHistoryController.Invoke)
            .WithOpenApi()
            .WithDescription("Get full scan history including food & nutrients.");
    }

    private static void MapUserRoutes(WebApplication app)
    {
        var userRoutes = app
            .MapGroup("/api/users")
            .WithTags("Users")
            .WithDescription("All endpoints related to Taskly users.")
            .WithOpenApi();

        userRoutes
            .MapPost("/", CreateUserController.Invoke)
            .WithDescription("Create a new user.")
            .WithName(nameof(CreateUserController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
            .MapGet("/{userId}", SearchUserByIdController.Invoke)
            .WithDescription("Get a user by their ID.")
            .WithName(nameof(SearchUserByIdController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
            .MapGet("/", SearchAllUsersController.Invoke)
            .WithDescription("Get all users .")
            .WithName(nameof(SearchAllUsersController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }
    
    private static void MapAnalyseRoutes(WebApplication app)
    {
        var analyseRoutes = app.MapGroup("/api/analyses")
            .WithTags("Analyses")
            .WithDescription("Endpoints for managing health analyses.")
            .WithOpenApi();

        analyseRoutes
            .MapPost("/", CreateAnalyseController.Invoke)
            .WithDescription("Create a new health analyse.")
            .WithName(nameof(CreateAnalyseController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
            
        analyseRoutes
            .MapGet("/user/{userId}/stats", GetBodyStatsController.Invoke)
            .WithDescription("Get the latest body statistics for a user.")
            .WithName(nameof(GetBodyStatsController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        analyseRoutes
            .MapPut("/definitions/{id}/goal", UpdateBodyStatController.Invoke)
            .WithDescription("Update the goal for a specific body stat definition.")
            .WithName(nameof(UpdateBodyStatController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }
}