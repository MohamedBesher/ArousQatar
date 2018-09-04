using System;
using AutoMapper;
using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core.Dtos;
using Saned.ArousQatar.Data.Core.Models;
#pragma warning disable 672

namespace Saned.ArousQatar.Api.Infrastructure.Mapping
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName => "DomainToViewModelMappings";


        protected override void Configure()

        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<AdvertisementDto, AdvertismentViewModel>();
            CreateMap<AdvertisementDto, AdViewModel>();
            CreateMap<AdvertisementDto, Advertisment>();
            CreateMap<AdViewModel, Advertisment>();
            CreateMap<Advertisment, AdvertismentViewModel>();
            CreateMap<AdvertismentViewModel, Advertisment>();
            CreateMap<AdvertismentImage, AdvertisementImageViewModel>();
            CreateMap<AdvertisementImageViewModel, AdvertismentImage>();
            CreateMap<AdvertismentPrice, AdvertismentPriceViewModel>();
            CreateMap<BankAccount, BankAccountViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<Complaint, ComplaintViewModel>();
            CreateMap<ComplaintViewModel, Complaint> ();
            CreateMap<ComplaintDto, ComplaintViewModel>();
            CreateMap<ContactInformation, ContactInformationViewModel>();
            CreateMap<ContactType, ContactTypeViewModel>();
            CreateMap<Favorite, FavoriteViewModel>();
            CreateMap<Like, LikeViewModel>();
            CreateMap<CommentDto, CommentViewModel>();
            CreateMap<FavoriteDto, FavoriteViewModel>();
            CreateMap<LikeDto, LikeViewModel>();
            CreateMap<CategoryDto, CategoryViewModel>();
            CreateMap<CategoryAllDto, CategoryViewModel>();
            CreateMap<AdvertisementSmallDto, AdvertisementSmallViewModel>();
            CreateMap<AdvertisementPriceDto, AdvertismentPriceViewModel>();
            CreateMap<ClientViewModel, RegisterUserData>();
            CreateMap<RequestViewModel, ChatRequest>();
            CreateMap<ChatRequest, RequestViewModel>();
            CreateMap<MessageViewModel, ChatMessage>();
            CreateMap<AdvertismentImage, AdImage>();
            CreateMap<AdvertismentTransaction, AdvertismentTransactionViewModel>();
            CreateMap<AdvertismentTransactionViewModel, AdvertismentTransaction>();



        }
    }
}