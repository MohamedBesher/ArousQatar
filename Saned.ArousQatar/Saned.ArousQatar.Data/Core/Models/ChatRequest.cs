using System;

namespace Saned.ArousQatar.Data.Core.Models
{

    public class ChatRequest : IEntityBase
    {

       public ChatRequest()
        {
            RequestDate = DateTime.Now;
        }
        public int Id { get; set; }
        public int AdvertismentId { get; set; }
        // Who Request Chat    
        public string RequestAuthorId { get; set; }  
        public DateTime RequestDate { get; private set; }


        #region virtual
        public virtual ApplicationUser RequestAuthor { get; set; }
        public virtual Advertisment Advertisment { get; set; }
        #endregion



 

    }

}