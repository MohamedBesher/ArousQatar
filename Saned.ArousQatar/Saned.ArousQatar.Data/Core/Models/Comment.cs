using System;
using System.Collections.Generic;

namespace Saned.ArousQatar.Data.Core.Models
{
    public class Comment : IEntityBase
    {
        public Comment()
        {
            Comments = new List<Comment>();
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }

        public int AdvertismentId { get; set; }
        public virtual Advertisment Advertisment { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int? CommentParentId { get; set; }
        public virtual Comment CommentParent { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
