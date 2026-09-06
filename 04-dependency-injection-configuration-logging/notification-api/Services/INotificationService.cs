using NotificationApi.Models;

namespace NotificationApi.Services;

public interface INotificationService
{
    NotificationResult Send(NotificationRequest request);
}
