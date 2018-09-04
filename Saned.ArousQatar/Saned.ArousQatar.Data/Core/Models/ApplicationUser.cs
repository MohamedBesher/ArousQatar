using System.Collections.Generic;

namespace Saned.ArousQatar.Data.Core.Models
{
    public partial class ApplicationUser
    {
        public string Name { get; set; }

        public ICollection<Advertisment> Advertisments { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public string ConfirmedEmailToken { get; set; }
        public string ResetPasswordlToken { get; set; }
        public bool? IsDeleted { get; set; }
        public string PhotoUrl { get; set; }
        public bool? IsApprove { get; set; }

        public bool Status { get; set; } = false;
    }
}
