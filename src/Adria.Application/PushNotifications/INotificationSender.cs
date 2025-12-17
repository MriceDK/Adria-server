namespace Adria.Application.PushNotifications;

public interface INotificationSender
{
    Task Send(string title, string body);
}