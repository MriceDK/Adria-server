using Adria.Domain.PushNotifications;
using WebPush;

namespace Adria.Main.Workers;

public class NotificationWorker : BackgroundService
{
    private static IServiceProvider _serviceProvider = null!;
    private static ILogger<NotificationWorker> _logger = null!;
    private static IConfiguration _configuration = null!;

    public NotificationWorker(
        IServiceProvider serviceProvider,
        ILogger<NotificationWorker> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
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
                    await SendNotifications(
                        "Nutriscan always with you!",
                        "Don't forget to hydrate."
                    );
                }
                else if (executionCount == 3)
                {
                    await SendNotifications(
                        "Feeling Hungry?",
                        "Time for a healthy snack."
                    );
                }
                else if (executionCount == 5)
                {
                    await SendNotifications(
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
                _logger.LogError(ex, "Notification error.");
            }
        }
    }

    public static async Task SendNotifications(string title, string body)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IPushSubscriptionRepository>();

        var subscriptions = await repository.GetAll();
        if (subscriptions.Count == 0) return;

        _logger.LogInformation(
            "Found {SubscriptionCount} users to notify.",
            subscriptions.Count
        );

        var subject = "mailto:admin@adria.com";
        var publicKey = _configuration["Vapid:PublicKey"];
        var privateKey = _configuration["Vapid:PrivateKey"];
        var vapidDetails = new VapidDetails(subject, publicKey, privateKey);

        var webPushClient = new WebPushClient();

        foreach (var sub in subscriptions)
        {
            try
            {
                var pushSubscription = new WebPush.PushSubscription(
                    sub.Endpoint,
                    sub.P256dh,
                    sub.Auth
                );

                var payload = System.Text.Json.JsonSerializer.Serialize(new
                {
                    title,
                    body
                });

                await webPushClient.SendNotificationAsync(
                    pushSubscription,
                    payload,
                    vapidDetails
                );

                _logger.LogInformation(
                    "Notification sent to user {UserId}",
                    sub.UserId
                );
            }
            catch (WebPushException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Gone)
            {
                _logger.LogWarning(
                    "Subscription expired for user {UserId}. Deleting.",
                    sub.UserId
                );
                await repository.Delete(sub.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send notification to user {UserId}",
                    sub.UserId
                );
            }
        }
    }
}
