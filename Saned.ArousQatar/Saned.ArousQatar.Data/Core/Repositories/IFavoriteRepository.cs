using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IFavoriteRepository : IEntityBaseRepository<Favorite>
    {
        Task<List<FavoriteDto>> GetAllAsync(string username, int start = 0, int number = 10);

        Task<FavoriteDto> GetSingleAsync(int id);
        Task<Favorite> GetFavoriteAsync(int id);
        Task<Favorite> GetSingleAsyncByUser(int id, string userId);
    }
}