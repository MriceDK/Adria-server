using Adria.Domain.PushNotifications;
using WebPush;

namespace Adria.Main.Workers;

public class NotificationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationWorker> _logger;
    private readonly IConfiguration _configuration;

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
        _logger.LogInformation("Notification Worker started running...");

        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await SendNotifications();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending notifications.");
            }
        }
    }

    private async Task SendNotifications()
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IPushSubscriptionRepository>();

        var subscriptions = await repository.GetAll();
        if (subscriptions.Count == 0) return;

        _logger.LogInformation($"Found {subscriptions.Count} users to notify.");

        // VAPID Keys (It should be in appsettings.json)
        // Client and backend keys should be same
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
                    title = "Don't forget drink water! 💧", 
                    body = $"Hi, it's perfect time to drink water! : {DateTime.Now:HH:mm}" 
                });

                await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
                _logger.LogInformation($"Notification sent to {sub.UserId}");
            }
            catch (WebPushException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Gone)
            {
                _logger.LogWarning($"Subscription expired for user {sub.UserId}. Deleting...");
                await repository.Delete(sub.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send to {sub.UserId}");
            }
        }
    }
}