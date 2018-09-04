using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IAdvertisementTransactionRepository : IEntityBaseRepository<AdvertismentTransaction>
    {

       // int Add(AdvertismentTransaction transaction);

        List<AdvertismentTransaction> GetAllTransactionsByAdvertisementId(int id);

    }
}
