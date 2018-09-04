using System;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Core.Repositories;
using Saned.ArousQatar.Data.Persistence.Infrastructure;
using Saned.ArousQatar.Data.Persistence.Repositories;
using System.Threading.Tasks;
using Saned.ArousQatar.Data.Core.Dtos;
using Saned.HandByHand.Data.Core.Repositories;
using Saned.HandByHand.Data.Persistence.Repositories;

namespace Saned.ArousQatar.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        private readonly IDbFactory _dbFactory;

        public ICategoryRepository Categories { get; private set; }
        public IAdvertisementRepository Advertisements { get; }
        public IAdvertisementImageRepository AdvertisementImages { get; }
        public IAdvertisementPriceRepository AdvertisementPrice { get; }
        public IBankAccountRepository BankAccounts { get; }
        public ICommentRepository Comments { get; }
        public IComplaintRepository Complaints { get; }
        public IContactInformationRepository ContactInformations { get; }
        public IContactTypeRepository ContactTypes { get; }
        public IFavoriteRepository Favorites { get; }
        public ILikeRepository Likes { get; }
        public IErrorRepository Errors { get; }
        public IEmailSettingRepository EmailSetting { get; }
        public IApplicationUserRepositoryAsync User { get; }
        public IAuthRepository AuthRepository { get; }
        public IChatRequestRepositoryAsync ChatRequest { get; }
        public IContactUsMessageRepository ContactUsMessage { get; }

        public IAdvertisementTransactionRepository AdvertisementTransactions { get; }


        //public INotificationRepository Notification { get; }

        public UnitOfWork(
            IDbFactory dbFactory,
            ICategoryRepository categories,
            IErrorRepository errors,
            IAdvertisementRepository advertisements,
            IAdvertisementImageRepository advertisementImages,
            IAdvertisementPriceRepository advertisementPrice,
            IBankAccountRepository bankAccounts,
            ICommentRepository comments,
            IComplaintRepository complaints,
            IContactInformationRepository contactInformations,
            IContactTypeRepository contactTypes,
            IFavoriteRepository favorites,
            ILikeRepository likes,
            IEmailSettingRepository emailSetting,
            IChatRequestRepositoryAsync chatRequest,
            IContactUsMessageRepository contactUsMessage,
            IAuthRepository authRepository//,
            //INotificationRepository notificationRepository
            )
        {
            _dbFactory = dbFactory;
            Categories = categories;
            Errors = errors;
            Advertisements = advertisements;
            AdvertisementImages = advertisementImages;
            AdvertisementPrice = advertisementPrice;
            BankAccounts = bankAccounts;
            Comments = comments;
            Complaints = complaints;
            ContactInformations = contactInformations;
            ContactTypes = contactTypes;
            Favorites = favorites;
            Likes = likes;
            EmailSetting = emailSetting;
            AuthRepository = authRepository;
            ChatRequest = chatRequest;
            ContactUsMessage = contactUsMessage;
            //Notification = notificationRepository;
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbFactory = new DbFactory();
      
            Categories = new CategoryRepository(_dbFactory);
            Errors = new ErrorRepository(_dbFactory);
            Advertisements = new AdvertisementRepository(_dbFactory);
            AdvertisementImages = new AdvertisementImageRepository(_dbFactory);
            AdvertisementPrice = new AdvertismentPriceRepository(_dbFactory);
            AdvertisementTransactions = new AdvertisementTransactionRepository(_dbFactory);
            BankAccounts = new BankAccountRepository(_dbFactory);
            Comments = new CommentRepository(_dbFactory);
            Complaints = new ComplaintRepository(_dbFactory);
            ContactInformations = new ContactInformationRepository(_dbFactory);
            ContactTypes = new ContactTypeRepository(_dbFactory);
            Favorites = new FavoriteRepository(_dbFactory);
            Likes = new LikeRepository(_dbFactory);
            EmailSetting = new EmailSettingRepository();
            ChatRequest = new ChatRequestRepositoryAsync(_context);
            ContactUsMessage = new ContactUsMessageRepository(_dbFactory);
            User = new ApplicationUserRepositoryAsync(_context);
            //Notification = new NotificationRepository(_context);
        }

        public UnitOfWork()
        {
            _context = new ApplicationDbContext();
            _dbFactory = new DbFactory();
            Categories = new CategoryRepository(_dbFactory);
            Errors = new ErrorRepository(_dbFactory);
            Advertisements = new AdvertisementRepository(_dbFactory);
            AdvertisementTransactions = new AdvertisementTransactionRepository(_dbFactory);
            AdvertisementImages = new AdvertisementImageRepository(_dbFactory);
            AdvertisementPrice = new AdvertismentPriceRepository(_dbFactory);
            BankAccounts = new BankAccountRepository(_dbFactory);
            Comments = new CommentRepository(_dbFactory);
            Complaints = new ComplaintRepository(_dbFactory);
            ContactInformations = new ContactInformationRepository(_dbFactory);
            ContactTypes = new ContactTypeRepository(_dbFactory);
            Favorites = new FavoriteRepository(_dbFactory);
            Likes = new LikeRepository(_dbFactory);
            EmailSetting = new EmailSettingRepository();
            User = new ApplicationUserRepositoryAsync(_context);
            ChatRequest = new ChatRequestRepositoryAsync(_context);
            ContactUsMessage = new ContactUsMessageRepository(_dbFactory);

            //Notification = new NotificationRepository(_context);
        }


        public ApplicationDbContext DbContext => _context ?? (_context = _dbFactory.Init());

    

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this._context == null)
            {
                return;
            }

            this._context.Dispose();
            this._context = null;
        }

    }
}
