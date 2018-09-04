using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class CommentRepository : EntityBaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<CommentDto>> GetAllAsync(int advId,int PageNumber=1 , int PageSize=8,string Filter=null)
        {

            return (await DbContext.Database.SqlQuery<CommentDto>("EXEC CommentGetAll @Id , @PageNumber, @PageSize, @Filter"
                , new SqlParameter("Id", SqlDbType.Int) { Value = advId }
                , new SqlParameter("PageNumber", SqlDbType.Int) { Value = PageNumber }
                , new SqlParameter("PageSize", SqlDbType.Int) { Value = PageSize },
                Getparamter(Filter, "Filter")).ToListAsync());
        }

        public async Task<List<CommentDto>> GetPagedComments( int PageNumber = 1, int PageSize = 8, string Filter = null)
        {
            return (await DbContext.Database.SqlQuery<CommentDto>("EXEC Comments_GetPagedComments @PageNumber, @PageSize, @Filter"
                , new SqlParameter("PageNumber", SqlDbType.Int) { Value = PageNumber }
                , new SqlParameter("PageSize", SqlDbType.Int) { Value = PageSize },
                Getparamter(Filter, "Filter")).ToListAsync());
        }

        public async Task<List<CommentDto>> GetAllReplysAsync(int comId)
        {
            return (await DbContext.Database.SqlQuery<CommentDto>("EXEC CommentReplyGetAll @id"
                 , new SqlParameter("id", SqlDbType.Int) { Value = comId }).ToListAsync());
        }

        public async Task<CommentDto> GetSingleDtoAsync(int id)
        {
            return (await DbContext.Database.SqlQuery<CommentDto>("EXEC CommentGetSingle @id"
                 , new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefaultAsync());
        }

        public async Task<Comment> GetSingleAsync(int id)
        {
            return (await DbContext.Database.SqlQuery<Comment>("EXEC CommentGetSingleWithoutUser @id"
                 , new SqlParameter("id", SqlDbType.Int) { Value = id }).FirstOrDefaultAsync());
        }

        public async Task<int> DeleteAllReplys(int commentdId)
        {
            return (await DbContext.Database.SqlQuery<int>("EXEC CommentDeleteAllReplays @id"
                 , new SqlParameter("id", SqlDbType.Int) { Value = commentdId }).FirstOrDefaultAsync());
        }
    }
}
