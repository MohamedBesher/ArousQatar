using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class AdvertisementTransactionRepository : EntityBaseRepository<AdvertismentTransaction>, IAdvertisementTransactionRepository
    {
        public AdvertisementTransactionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<AdvertismentTransaction> GetAllTransactionsByAdvertisementId(int id)
        {
            return DbContext.AdvertismentTransactions.Where(u => u.AdvertismentId == id).ToList();
        }
    }
}
