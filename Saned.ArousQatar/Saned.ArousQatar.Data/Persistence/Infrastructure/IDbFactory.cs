namespace Saned.ArousQatar.Data.Persistence.Infrastructure
{
    public interface IDbFactory
    {
        ApplicationDbContext Init();
    }
}
