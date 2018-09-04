using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<WebApplication1.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Advertisment> Advertisments { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.AdvertismentPrice> AdvertismentPrices { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.AdvertismentImage> AdvertismentImages { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.BankAccount> BankAccounts { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Complaint> Complaints { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ContactInformation> ContactInformations { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ContactType> ContactTypes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Favorite> Favorites { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Like> Likes { get; set; }
    }
}