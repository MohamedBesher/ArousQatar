using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface ILikeRepository : IEntityBaseRepository<Like>
    {
        Task<List<LikeDto>> GetAllAsync(int advId);
        Task<Like> GetSingleAsyncByUser(int id, string userId);
        Task<Like> GetSingleAsync(int id);
        void Save(Like bo);
    }
}