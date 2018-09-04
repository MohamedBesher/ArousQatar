using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Saned.ArousQatar.Data.Core.Models
{
    public class ChatHeader : IEntityBase
    {
        public ChatHeader()
        {
            CreateDate = DateTime.Now;
        }

        public int RequestId { get; set; }
        public string UserOneId { get; set; }
        public string UserTwoId { get; set; }

        public int Id { get; set; }
        public DateTime CreateDate { get; private set; }



        #region virtual
        public virtual ApplicationUser UserOne { get; set; }
        public virtual ApplicationUser UserTwo { get; set; }
        public virtual ChatRequest Request { get; set; }
        public virtual ICollection<ChatMessage> Messages { get; set; }
        #endregion


    }

}