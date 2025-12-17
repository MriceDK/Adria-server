using Adria.Application.PushNotifications;

namespace Adria.Infrastructure.PushNotifications;

public sealed class NotificationWorker : BackgroundService
{
    private readonly INotificationSender _notificationSender;
    private readonly ILogger<NotificationWorker> _logger;

    public NotificationWorker(
        INotificationSender notificationSender,
        ILogger<NotificationWorker> logger)
    {
        _notificationSender = notificationSender;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));
        int executionCount = 0;

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            executionCount++;

            try
            {
                if (executionCount == 1)
                {
                    await _notificationSender.Send(
                        "Nutriscan always with you!",
                        "Don't forget to hydrate."
                    );
                }
                else if (executionCount == 3)
                {
                    await _notificationSender.Send(
                        "Feeling Hungry?",
                        "Time for a healthy snack."
                    );
                }
                else if (executionCount == 5)
                {
                    await _notificationSender.Send(
                        "Time to Move!",
                        "Stretch your legs!"
                    );
                }

                if (executionCount >= 15)
                {
                    executionCount = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Meaningful message with context");
            }
        }
    }
}