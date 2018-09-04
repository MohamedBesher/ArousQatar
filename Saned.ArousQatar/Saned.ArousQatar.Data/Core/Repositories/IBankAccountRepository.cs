using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IBankAccountRepository : IEntityBaseRepository<BankAccount>
    {
        Task<List<BankAccount>> GetAllAsync(int start = 0, int number = 10, string filter = "");

        Task<BankAccount> GetSingleAsync(int id);
    }
}