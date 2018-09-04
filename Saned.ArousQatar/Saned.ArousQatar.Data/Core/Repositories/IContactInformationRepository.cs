using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IContactInformationRepository : IEntityBaseRepository<ContactInformation>
    {
      //  Task<List<ContactInformation>> GetAllAsync(int start = 0, int number = 10);
        Task<List<ContactInformationDto>> GetAllAsync(int PageNumber = 1, int PageSize = 10, string Filter = null);
 

        Task<ContactInformation> GetSingleAsync(int id);

        List<string > GetAllWithTypes(int id);


    }
}