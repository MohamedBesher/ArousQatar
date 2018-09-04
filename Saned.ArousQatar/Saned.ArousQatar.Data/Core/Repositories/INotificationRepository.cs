using System.Collections.Generic;
using System.Threading.Tasks;
using Saned.HandByHand.Data.Core.Dtos;
using Saned.HandByHand.Data.Core.Models.Notifications;

namespace Saned.HandByHand.Data.Core.Repositories
{
    public interface INotificationRepository
    {
        Task<List<NotificationLog>> GetNotifications(string UserId);

        int AddNotificaiton(PushNotification Notification,string RecieverId, bool checkStatus = false);
        Task<List<string>> GetNotifictionReciverList(int NotificationId);


        Task<List<NotificationDto>> GetNotificationList(string lang, int page, int offset, string userId);


        Task<int> MarkAsRead(string notificationIds);
        
    }
}
