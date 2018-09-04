using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Repositories
{
    public class ChatRequestRepositoryAsync : IChatRequestRepositoryAsync
    {

        private readonly ApplicationDbContext _context;
        public ChatRequestRepositoryAsync(ApplicationDbContext context)
        {
            // _context = context;
            _context = new ApplicationDbContext();
        }

        public async Task<List<ChatListDto>> GetChatByUserId(string userId, int advertisementId, int pageNumber = 1, int pageSize = 8)
        {
            SqlParameter userIdParamter = new SqlParameter("@UserId", userId);
            SqlParameter pageNumberParameter = new SqlParameter("@PageNumber", pageNumber);
            SqlParameter pageSizeParameter = new SqlParameter("@PageSize", pageSize);
            SqlParameter advertismentIdParameter = new SqlParameter("@AdvertismentId", advertisementId);

            return (await _context.Database.SqlQuery<ChatListDto>("ChatHeader_ListByUserId @UserId,@PageNumber,@PageSize,@AdvertismentId",
                userIdParamter, pageNumberParameter, pageSizeParameter, advertismentIdParameter).ToListAsync());
        }


        public async Task<List<ChatMessageDto>> GetMessage(int chatid, int pageNumber = 1, int pageSize = 8, DateTime? lastSendDate = null)
        {
            SqlParameter chatIdParameter = new SqlParameter("@ChatId", chatid);
            SqlParameter pageNumberParameter = new SqlParameter("@PageNumber", pageNumber);
            SqlParameter pageSizeParameter = new SqlParameter("@PageSize", pageSize);
            var dateParameter = Getparamter(lastSendDate, "LastSendDate");
            return (await _context.Database.SqlQuery<ChatMessageDto>("ChatMessages_SelectList @PageNumber,@PageSize,@LastSendDate,@ChatId", pageNumberParameter, pageSizeParameter, dateParameter, chatIdParameter).ToListAsync());
        }

        public async Task<ChatHeader> GetHeader(int id)
        {
            var res = await _context.Database.SqlQuery<ChatHeader>("ChatHeader_ById @Id", new SqlParameter("Id", SqlDbType.Int) { Value = id }).FirstAsync();
            return (res);

        }

        public async Task<ChatHeader> GetHeaderbyChatRequest(long id)
        {
            return await _context.ChatHeaders.SingleOrDefaultAsync(x => x.RequestId == id);
        }

        public async Task<ChatRequest> GetRequest(int id)
        {
            return (await _context.Database.SqlQuery<ChatRequest>("ChatRequests_ById @Id", new SqlParameter("Id", SqlDbType.Int) { Value = id }).FirstAsync());

        }

        public async Task<int> ChatRequestAdd(ChatRequest request)
        {
            var res = await _context.Database.SqlQuery<int>(
                       "ChatRequests_Add @AdvertismentId,@RequestAuthorId,@RequestDate ",
                        new SqlParameter("@AdvertismentId", request.AdvertismentId),
                        new SqlParameter("@RequestAuthorId", request.RequestAuthorId),
                        new SqlParameter("@RequestDate", request.RequestDate)
                        ).FirstAsync();


            return (res);
        }

        public async Task<int> AddMessage(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            return await _context.CommitAsync();
        }

        public async Task<List<ChatListDto>> GetChatByAdvertisment(int advertismentId, int pageNumber = 1, int pageSize = 8)
        {
            SqlParameter advertismentIdParameter = new SqlParameter("@AdvertismentId", advertismentId);
            SqlParameter pageNumberParameter = new SqlParameter("@PageNumber", pageNumber);
            SqlParameter pageSizeParameter = new SqlParameter("@PageSize", pageSize);
            var res = await _context.Database.SqlQuery<ChatListDto>("ChatHeader_ListByAdvertismentId @AdvertismentId,@PageNumber,@PageSize", advertismentIdParameter, pageNumberParameter, pageSizeParameter).ToListAsync();
            return (res);
        }

        private static SqlParameter Getparamter(object id, string parameterName)
        {
            var accountTypeIdParameter = id == null
                ? new SqlParameter("@" + parameterName, DBNull.Value)
                : new SqlParameter("@" + parameterName, id);
            return accountTypeIdParameter;
        }


    }
}