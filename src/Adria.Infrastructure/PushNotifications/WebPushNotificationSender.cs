using Adria.Application.PushNotifications;
using Adria.Domain.PushNotifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebPush;

namespace Adria.Infrastructure.PushNotifications;

public sealed class WebPushNotificationSender : INotificationSender
{

    private readonly IConfiguration _configuration;
    private readonly ILogger<WebPushNotificationSender> _logger;
    private static IServiceProvider? _serviceProvider;


    public WebPushNotificationSender(
        IPushSubscriptionRepository repository,
        IConfiguration configuration,
        ILogger<WebPushNotificationSender> logger,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;

    }

    public async Task Send(string title, string body)
    {
        using var scope = _serviceProvider!.CreateScope();
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
                    title = title, 
                    body = body
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
