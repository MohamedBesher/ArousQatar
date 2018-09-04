using Saned.ArousQatar.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.HandByHand.Data.Core.Models.Notifications
{
    /// <summary>
    /// This works as a holder to multipe reciver of a single Notification (In case many users got it )
    /// </summary>
    public class NotificationLog 
    {
        public NotificationLog()
        {
            

        }
        [Key]
        public int Id { get; set; }
        public int NotificationMessageId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual PushNotification NotificationMessage { get; set; }



    }
}
