using Saned.ArousQatar.Data.Core.Repositories;
using Saned.HandByHand.Data.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IAdvertisementRepository Advertisements { get; }
        IAdvertisementImageRepository AdvertisementImages { get; }
        IAdvertisementPriceRepository AdvertisementPrice { get; }
        IAdvertisementTransactionRepository AdvertisementTransactions { get; }
        IBankAccountRepository BankAccounts { get; }
        ICommentRepository Comments { get; }
        IComplaintRepository Complaints { get; }
        IContactInformationRepository ContactInformations { get; }
        IContactTypeRepository ContactTypes { get; }
        IFavoriteRepository Favorites { get; }
        ILikeRepository Likes { get; }
        IErrorRepository Errors { get; }
        IEmailSettingRepository EmailSetting { get; }
        IApplicationUserRepositoryAsync User { get; }
        IChatRequestRepositoryAsync ChatRequest { get; }
        IContactUsMessageRepository ContactUsMessage { get; }
        //IAuthRepository AuthRepository { get; }

        //INotificationRepository Notification { get; }

        void Commit();
        
        Task<int> CommitAsync();
    }
}
