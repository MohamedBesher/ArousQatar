using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IContactUsMessageRepository : IEntityBaseRepository<ContactUsMessage>
    {
        Task<List<ContactUsMessageDto>> GetAllAsync(int pageNumber=1, int pageSize=10, string filter=null, bool? isArchieve=null);
        Task<ContactUsMessage> GetSingleAsync(int id);
    }
}