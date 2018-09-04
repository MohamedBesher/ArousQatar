using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core.Repositories
{
    public interface IChatRequestRepositoryAsync 
    {
        Task<List<ChatListDto>> GetChatByUserId(string userId, int advertisementId, int pageNumber = 1, int pageSize = 8);
        Task<List<ChatListDto>> GetChatByAdvertisment(int advertismentId, int pageNumber = 1, int pageSize = 8);
        Task<List<ChatMessageDto>> GetMessage(int chatid, int pageNumber = 1, int pageSize = 8, DateTime? lastSendDate = null);
        Task<ChatHeader> GetHeader(int id);
        Task<int> ChatRequestAdd(ChatRequest request);
        Task<int> AddMessage(ChatMessage message);
        Task<ChatRequest> GetRequest(int id);

        Task<ChatHeader> GetHeaderbyChatRequest(long id);
    }
}