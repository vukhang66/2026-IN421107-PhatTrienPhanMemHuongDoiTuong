namespace EduHub.Ch02.Notifications;

public interface INotificationServiceFactory
{
    INotificationService Create(NotificationChannel channel);
}
