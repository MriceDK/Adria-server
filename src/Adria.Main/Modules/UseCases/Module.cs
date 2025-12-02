using Adria.Application.Analyses;
using Adria.Application.BodyStats;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.FoodComposition;
using Adria.Application.Subscriptions;
using Adria.Application.Users;
using Adria.Infrastructure.Persistence.Queries;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
                .AddScoped<IUseCase<CreateSubscriptionInput, Task<Guid>>, CreateSubscription>()
                .AddScoped<IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>, SearchAllSubscriptions>()
                .AddScoped<IUseCase<SearchSubscriptionByIdInput, Task<SubscriptionData>>, SearchSubscriptionById>()
                .AddScoped<IUseCase<SearchUserByIdInput, Task<UserData>>, SearchUserById>()
                .AddScoped<IUseCase<Task<IReadOnlyCollection<UserData>>>, SearchAllUsers>()
                .AddScoped<IUseCase<CreateUserInput, Task<Guid>>, CreateUser>()                
                .AddScoped<IUseCase<CreateFoodCompositionInput, Task>, CreateFoodComposition>()
                .AddScoped<IUseCase<SearchFoodCompositionsByFoodIdInput, Task<IReadOnlyCollection<FoodCompositionData>>>, SearchFoodCompositionsByFoodName>()
                .AddScoped<IUseCase<SearchFoodCompositionsByNutrientIdInput, Task<IReadOnlyCollection<FoodCompositionData>>>, SearchFoodCompositionsByNutrientId>()
                .AddScoped<IUseCase<RemoveFoodCompositionInput, Task>, RemoveFoodComposition>()
                .AddScoped<IUseCase<CreateAnalyseInput, Task<Guid>>, CreateAnalyse>()
                .AddScoped<IUseCase<GetLatestBodyStatsInput, Task<IReadOnlyCollection<BodyStatData>>>, GetLatestBodyStats>();
    }
}