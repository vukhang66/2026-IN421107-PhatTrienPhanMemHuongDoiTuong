namespace EduHub.Ch02.Notifications;

public class SlackNotificationService : INotificationService
{
    public void Send(string to, string message) =>
        Console.WriteLine($"  [SLACK] to {to}: {message}");
}
