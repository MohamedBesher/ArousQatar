using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class BankAccountRepository : EntityBaseRepository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<BankAccount>> GetAllAsync(int start = 0, int number = 10, string filter = "")
        {
            return (await GetAllAsync("BankAccount", start, number, filter));
        }

        public async Task<BankAccount> GetSingleAsync(int id)
        {
            return (await GetSingleAsync("BankAccount", id));
        }


    }
}
