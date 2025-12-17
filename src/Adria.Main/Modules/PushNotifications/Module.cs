using Adria.Application.PushNotifications;
using Adria.Infrastructure.PushNotifications;

namespace Adria.Main.Modules.PushNotifications;

public static class PushNotificationModule
{

    public static IServiceCollection AddPersistenceModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSingleton<INotificationSender, LogNotificationSender>();
        services.AddHostedService<NotificationWorker>();

        return services;
    }
}