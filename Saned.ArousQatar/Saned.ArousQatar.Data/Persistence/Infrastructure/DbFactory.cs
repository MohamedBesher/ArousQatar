namespace Saned.ArousQatar.Data.Persistence.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        ApplicationDbContext dbContext;

        public ApplicationDbContext Init()
        {
            return dbContext ?? (dbContext = new ApplicationDbContext());
        }

        protected override void DisposeCore()
        {
            dbContext?.Dispose();
        }
    }
}
