using Saned.ArousQatar.Data.Core.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Advertisment> Advertisments { get; set; }
        DbSet<AdvertismentImage> AdvertismentImages { get; set; }
        DbSet<AdvertismentPrice> AdvertismentPrices { get; set; }
        DbSet<AdvertismentTransaction> AdvertismentTransactions { get; set; }
        DbSet<BankAccount> BankAccounts { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Complaint> Complaints { get; set; }
        DbSet<ContactType> ContactTypes { get; set; }
        DbSet<Favorite> Favorites { get; set; }
        DbSet<Like> Likes { get; set; }
        DbSet<ContactInformation> ContactInformations { get; set; }
        DbSet<Error> Errors { get; set; }
        DbSet<EmailSetting> EmailSettings { get; set; }

        void Commit();
        Task<int> CommitAsync();
    }
}