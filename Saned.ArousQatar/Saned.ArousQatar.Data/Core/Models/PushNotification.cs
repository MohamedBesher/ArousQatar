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
    /// A One push notification .. Every object rerpeset a push notification and a Database notification
    /// </summary>
    public class PushNotification
    {
        public PushNotification()
        {
            CreationDate = DateTime.Now;
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public string EnglishMessage { get; set; }
        public DateTime CreationDate { get; private set; }
        public bool Notified { get; set; }
        //  public string NotificationId { get; set; }
        public long ChatRequestId { get; set; }
        public  ChatRequest ChatRequest { get; set; }
    }
}
