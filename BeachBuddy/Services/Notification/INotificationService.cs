using System.Threading.Tasks;
using BeachBuddy.Entities;
using BeachBuddy.Enums;

namespace BeachBuddy.Services.Notification
{
    public interface INotificationService
    {
        Task sendNotification(RequestedItem requestedItem,
            NotificationType notificationType,
            string notificationTitle,
            string notificationMessage,
            bool dataOnly);
    }
}