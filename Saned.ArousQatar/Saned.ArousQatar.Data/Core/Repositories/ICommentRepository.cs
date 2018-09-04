using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface ICommentRepository : IEntityBaseRepository<Comment>
    {
        Task<List<CommentDto>> GetAllAsync(int advId, int PageNumber = 1, int PageSize = 8, string Filter = null);
        Task<List<CommentDto>> GetAllReplysAsync(int comId);
        Task<List<CommentDto>> GetPagedComments(int PageNumber = 1, int PageSize = 8, string Filter = null);
        Task<CommentDto> GetSingleDtoAsync(int id);
        Task<Comment> GetSingleAsync(int id);
        Task<int> DeleteAllReplys(int commentdId);
    }
}