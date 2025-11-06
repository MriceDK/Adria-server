using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<CreateSubscriptionInput, Task<Guid>>, CreateSubscription>()
            .AddScoped<IUseCase<SearchSubscriptionByIdInput, Task<SubscriptionData>>, SearchSubscriptionById>()
            ;

    }
}
