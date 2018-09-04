using Saned.ArousQatar.Data.Persistence;
using Saned.ArousQatar.Data.Persistence.Repositories;
using Saned.HandByHand.Data.Core.Dtos;
using Saned.HandByHand.Data.Core.Models.Notifications;
using Saned.HandByHand.Data.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Saned.HandByHand.Data.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthRepository _repo;
        private string app_id = "21624373-9d13-4bd5-9c93-9564218ad7bc";
        private string auth_Id = "NzNjZGE3NGUtOWQ2Zi00ZTQ5LTg4OGQtZDVmN2E0NjM0ZTEz";
        private string rest_key = "MmNhNmI5ODktYjhjNi00NmNmLTkzYzktNWZlNDI2Y2JjYmM2";
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
            _repo = new AuthRepository();
        }
        public async Task<List<NotificationLog>> GetNotifications(string UserId)
        {
            return await _context.NotificationLogs.Where(x => x.ApplicationUserId == UserId).ToListAsync();
        }
        /// <summary>
        /// Add notification to data base
        /// </summary>
        /// <param name="notification">Object of notification</param>
        /// <param name="recieverId">Reciever Application User Id</param>
        public int AddNotificaiton(PushNotification notification, string recieverId, bool checkStatus = false)
        {
            //var list = await this.GetDevicesOfUser(recieverId);
            bool status = false;
            if (checkStatus)
            {
                status = _context.Users.FirstOrDefault(u => u.Id == recieverId).Status;
                if (status)
                    notification.Notified = true;
            }

            //if ( !status && list.Count > 0 && list != null)
            //{
            //    HashSet<string> reciversIds = new HashSet<string>();
            //    reciversIds.Add(list[0].DeviceId);
            //    OneSignalLibrary.Posting.Device reciever = new OneSignalLibrary.Posting.Device(reciversIds);


            //    Dictionary<string, string> messages = new Dictionary<string, string>();
            //    messages.Add("ar", notification.Message);
            //    messages.Add("en", notification.EnglishMessage);
            //    OneSignalLibrary.Posting.ContentAndLanguage content = new OneSignalLibrary.Posting.ContentAndLanguage(messages);
             // SignalClient.SendNotification(reciever, content, null, null);


            //}



            _context.Notifications.Add(notification);
            _context.NotificationLogs.Add(new NotificationLog()
            {
                ApplicationUserId = recieverId,
                NotificationMessage = notification
            });
            return 1;

        }
        public async Task<List<string>> GetNotifictionReciverList(int notificationId)
        {
            return await _context.NotificationLogs
                .Where(x => x.NotificationMessageId == notificationId)
                .Select(x => x.ApplicationUserId).ToListAsync();


        }
        public async Task<List<NotificationDto>> GetNotificationList(string lang, int page, int offset, string userId)
        {

            SqlParameter langParameter = new SqlParameter("@Lang", lang);
            SqlParameter pageNumberParameter = new SqlParameter("@PageNumber", page);
            SqlParameter pageSizeParameter = new SqlParameter("@PageSize", offset);
            SqlParameter userIdParameter = new SqlParameter("@UserId", userId);
            return (await _context.Database.SqlQuery<NotificationDto>("Notifications_Select_ListByUserId @UserId,@Lang,@PageNumber,@PageSize", userIdParameter, langParameter, pageNumberParameter, pageSizeParameter).ToListAsync());
        }
        public async Task<int> MarkAsRead(string notificationIds)
        {

            SqlParameter notificationIdsParameter = new SqlParameter("@NotificationIds", notificationIds);
            Task<int> affectedRows = _context.Database.SqlQuery<int>("Notifications_Mark_AsRead @NotificationIds",
                notificationIdsParameter).SingleAsync();
            return await affectedRows;
        }
        //public async Task SendNotificationsToUsers(string messageContent, string messageContentEn, string[] ids)
        //{
        //    foreach (string recieverId in ids)
        //    {
        //        var list = await this.GetDevicesOfUser(recieverId);
        //        Dictionary<string, string> messages = new Dictionary<string, string>();
        //        messages.Add("ar", messageContent);
        //        messages.Add("en", messageContentEn);
        //        OneSignalLibrary.Posting.ContentAndLanguage content = new OneSignalLibrary.Posting.ContentAndLanguage(messages);
        //        if (list.Count > 0)
        //        {
        //            HashSet<string> reciversIds = new HashSet<string>();
        //            reciversIds.Add(list[0].DeviceId);
        //            OneSignalLibrary.Posting.Device reciever = new OneSignalLibrary.Posting.Device(reciversIds);                  
        //            SignalClient.SendNotification(reciever, content, null, null);


        //        }
        //    }

        //}
    }
}
