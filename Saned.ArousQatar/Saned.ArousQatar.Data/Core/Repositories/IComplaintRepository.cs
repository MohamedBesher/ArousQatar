using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IComplaintRepository : IEntityBaseRepository<Complaint>
    {
        Task<List<ComplaintDto>> GetAllUsersComplaintsAsync(int start, int number);
        Task<List<ComplaintDto>> GetAllAdvertisementComplaintsAsync(int index, int rowNumber, string filter);
        Task<List<ComplaintDto>> GetAllUsersComplaintsArchieveAsync(int start, int number);
        Task<List<ComplaintDto>> GetAllAdvertisementComplaintsArchieveAsync(int start, int number);
        Task<Complaint> GetSingleAsyncByUser(int id, string userId);
        Task<Complaint> GetSingleAsync(int id);
    }
}