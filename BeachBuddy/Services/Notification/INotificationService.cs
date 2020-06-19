using System.Threading.Tasks;
using BeachBuddy.Entities;

namespace BeachBuddy.Services.Notification
{
    public interface INotificationService
    {
        Task sendNotification(RequestedItem requestedItem,
            string notificationTitle,
            string notificationMessage,
            bool dataOnly);
    }
}