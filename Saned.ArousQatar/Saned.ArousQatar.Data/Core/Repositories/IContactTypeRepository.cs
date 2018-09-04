using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IContactTypeRepository : IEntityBaseRepository<ContactType>
    {
        Task<List<ContactType>> GetAllAsync(int start = 0, int number = 10);
        Task<List<ContactType>> GetAllAsync();

        Task<ContactType> GetSingleAsync(int id);
        Task<bool> IsRelatedToContactInformation(int typeId);
    }
}