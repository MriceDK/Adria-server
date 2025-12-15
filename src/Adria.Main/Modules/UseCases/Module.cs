using Adria.Application.Analyses;
using Adria.Application.BodyStats;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Food;
using Adria.Application.FoodComposition;
using Adria.Application.PushNotifications;
using Adria.Application.Scanner;
using Adria.Application.Subscriptions;
using Adria.Application.Users;
using Adria.Application.Supplement;
using Adria.Application.Order;
using Adria.Infrastructure.Persistence.Queries;
using Adria.Infrastructure.Persistence.Repositories;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
        // --- Subscription & User Use Cases ---
        .AddScoped<IUseCase<CreateSubscriptionInput, Task<Guid>>, CreateSubscription>()
        .AddScoped<IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>, SearchAllSubscriptions>()
        .AddScoped<IUseCase<SearchSubscriptionByIdInput, Task<SubscriptionData>>, SearchSubscriptionById>()
        .AddScoped<IUseCase<SearchUserByIdInput, Task<UserData>>, SearchUserById>()
        .AddScoped<IUseCase<Task<IReadOnlyCollection<UserData>>>, SearchAllUsers>()
        .AddScoped<IUseCase<CreateUserInput, Task<Guid>>, CreateUser>()

        // --- Food Composition Use Cases ---
        .AddScoped<IUseCase<CreateFoodCompositionInput, Task>, CreateFoodComposition>()
        .AddScoped<IUseCase<SearchFoodCompositionsByFoodIdInput, Task<IReadOnlyCollection<FoodCompositionData>>>, SearchFoodCompositionsByFoodName>()
        .AddScoped<IUseCase<SearchFoodCompositionsByNutrientIdInput, Task<IReadOnlyCollection<FoodCompositionData>>>, SearchFoodCompositionsByNutrientId>()
        .AddScoped<IUseCase<RemoveFoodCompositionInput, Task>, RemoveFoodComposition>()

        // --- Analysis, Body Stat, and Push Use Cases ---
        .AddScoped<IUseCase<CreateAnalyseInput, Task<Guid>>, CreateAnalyse>()
        .AddScoped<IUseCase<UpdateBodyStatGoalInput, Task>, UpdateBodyStatGoal>()
        .AddScoped<IUseCase<SubscribeToPushInput, Task>, SubscribeToPush>()
        .AddScoped<IUseCase<GetLatestBodyStatsInput, Task<IReadOnlyCollection<BodyStatData>>>, GetLatestBodyStats>()
        
        // --- Food and Scanning Use Cases (Some are non-IUseCase classes) ---
        .AddScoped<IUseCase<FoodData, Task<Guid>>, CreateNewFood>()
        .AddScoped<GetAllFoods>()
        .AddScoped<GetRandomFood>()
        .AddScoped<GetFoodNutrients>()
        .AddScoped<IUseCase<GetFoodNutrientsInput, Task<IReadOnlyCollection<NutrientInfo>>>, GetFoodNutrients>()
        .AddScoped<ScanFood>()
        .AddScoped<GetScanHistory>()
        .AddScoped<IUseCase<GetScanHistoryInput, Task<IReadOnlyCollection<ScannedFoodResult>>>, GetScanHistory>()
        .AddScoped<RemoveScanFood>()
        
        // --- Supplement Use Cases ---
        .AddScoped<IUseCase<CreateSupplementInput, Task<Guid>>, CreateSupplement>()
        .AddScoped<IUseCase<SearchSupplementsByIdInput, Task<SupplementData?>>, SearchSupplementsById>()
        .AddScoped<IUseCase<Task<IReadOnlyCollection<SupplementData?>>>, SearchAllSupplements>()
        
        // --- Order Use Cases ---
        .AddScoped<IUseCase<CreateOrderInput, Task<Guid>>, CreateOrder>()
        .AddScoped<IUseCase<DeleteOrderInput, Task>, DeleteOrder>()
        .AddScoped<IUseCase<SearchOrderByIdInput, Task<OrderData?>>, SearchOrderById>()
        .AddScoped<IUseCase<SearchOrderByUserIdInput, Task<IReadOnlyCollection<OrderData>>>, SearchOrderByUserId>();
    }
}