namespace EduHub.Ch02.Notifications;

public interface INotificationService
{
    void Send(string to, string message);
}
