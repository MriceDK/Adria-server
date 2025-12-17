using Adria.Application.PushNotifications;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.PushNotifications;

public sealed class LogNotificationSender : INotificationSender
{
    private readonly ILogger<LogNotificationSender> _logger;

    public LogNotificationSender(ILogger<LogNotificationSender> logger)
    {
        _logger = logger;
    }

    public Task Send(string title, string body)
    {
        _logger.LogInformation("Push notification: {Title} - {Body}", title, body);
        return Task.CompletedTask;
    }
}