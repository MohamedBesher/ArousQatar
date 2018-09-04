using Microsoft.AspNet.Identity.EntityFramework;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Persistence.EntityConfigurations;
using Saned.HandByHand.Data.Core.Models.Notifications;
using System.Data.Entity;
using System.Threading.Tasks;


namespace Saned.ArousQatar.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Advertisment> Advertisments { get; set; }
        public DbSet<AdvertismentImage> AdvertismentImages { get; set; }
        public DbSet<AdvertismentPrice> AdvertismentPrices { get; set; }
        public DbSet<AdvertismentTransaction> AdvertismentTransactions { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<EmailSetting> EmailSettings { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ChatHeader> ChatHeaders { get; set; }
        public DbSet<ChatRequest> ChatRequests { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ContactUsMessage> ContactUsMessage { get; set; }
        public DbSet<PushNotification> Notifications { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }


        public ApplicationDbContext()
                    : base("Saned_ArousQatar", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
       //public static ApplicationDbContext Create()
       // {
       //    return new ApplicationDbContext();
       // }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public virtual async Task<int> CommitAsync()
        {
            return await SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryConfigurations());
            modelBuilder.Configurations.Add(new AdvertismentTransactionConfigurations());
            modelBuilder.Configurations.Add(new AdvertismentConfigurations());
            modelBuilder.Configurations.Add(new AdvertismentImageConfigurations());
            modelBuilder.Configurations.Add(new AdvertismentPriceConfigurations());
            modelBuilder.Configurations.Add(new ApplicationUserConfigurations());
            modelBuilder.Configurations.Add(new BankAccountConfigurations());
            modelBuilder.Configurations.Add(new CommentConfigurations());
            modelBuilder.Configurations.Add(new ComplaintConfigurations());
            modelBuilder.Configurations.Add(new ContactInformationConfigurations());
            modelBuilder.Configurations.Add(new ContactTypeConfigurations());
            modelBuilder.Configurations.Add(new FavoritesConfigurations());
            modelBuilder.Configurations.Add(new LikeConfigurations());
            modelBuilder.Configurations.Add(new ChatHeaderConfigurations());
            modelBuilder.Configurations.Add(new ChatMessageConfigurations());
            modelBuilder.Configurations.Add(new ChatRequestConfigurations());
            modelBuilder.Configurations.Add(new ContactUsMessageConfigurations());


            modelBuilder.Entity<Category>().MapToStoredProcedures();
            modelBuilder.Entity<Advertisment>().MapToStoredProcedures();
            modelBuilder.Entity<AdvertismentImage>().MapToStoredProcedures();
            modelBuilder.Entity<AdvertismentPrice>().MapToStoredProcedures();
            modelBuilder.Entity<AdvertismentTransaction>().MapToStoredProcedures();
            modelBuilder.Entity<BankAccount>().MapToStoredProcedures();
            modelBuilder.Entity<Comment>().MapToStoredProcedures();
            modelBuilder.Entity<Complaint>().MapToStoredProcedures();
            modelBuilder.Entity<ContactInformation>().MapToStoredProcedures();
            modelBuilder.Entity<ContactType>().MapToStoredProcedures();
            modelBuilder.Entity<Error>().MapToStoredProcedures();
            modelBuilder.Entity<Favorite>().MapToStoredProcedures();
            modelBuilder.Entity<Like>().MapToStoredProcedures();
            modelBuilder.Entity<ChatMessage>().MapToStoredProcedures();
            modelBuilder.Entity<ContactUsMessage>().MapToStoredProcedures();

            // modelBuilder.Entity<Category>().MapToStoredProcedures();

            base.OnModelCreating(modelBuilder);
        }

    }
}
