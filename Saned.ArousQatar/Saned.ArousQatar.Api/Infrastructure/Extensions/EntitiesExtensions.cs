using Saned.ArousQatar.Api.Models;
using Saned.ArousQatar.Data.Core.Models;

namespace Saned.ArousQatar.Api.Infrastructure.Extensions
{
    public static class EntitiesExtensions
    {
        public static void Update(this Category category, CategoryViewModel categoryVm)
        {
            category.Modify(categoryVm.Name, categoryVm.IconName, categoryVm.ImageUrl);
        }

        public static void Update(this Advertisment advertisment, AdvertismentViewModel advertismentVm)
        {
            advertisment.Modify(advertismentVm.Name, advertismentVm.Description,
                                advertismentVm.PaidEdPrice, advertismentVm.StartDate, advertismentVm.EndDate,
                                advertismentVm.CategoryId, advertismentVm.AdvertismentPriceId, advertismentVm.IsPaided, advertismentVm.IsActive, advertismentVm.Cost);
        }
        public static void UpdateAds(this Advertisment advertisment, AdvertismentViewModel advertismentVm)
        {
            advertisment.Modify(advertismentVm.Name, advertismentVm.Description,                               
                                advertismentVm.CategoryId, advertismentVm.IsActive, advertismentVm.Cost);
        }

        public static void Update(this AdvertismentImage advertismentImage,
            AdvertisementImageViewModel advertisementImageVm)
        {
            advertismentImage.Modify(advertisementImageVm.ImageUrl, advertisementImageVm.AdvertismentId);
        }

        public static void Update(this AdvertismentPrice advertismentPrice,
            AdvertismentPriceViewModel advertismentPriceVm)
        {
            advertismentPrice.Modify(advertismentPriceVm.Period, advertismentPriceVm.Price);
        }

        public static void Update(this BankAccount bankAccount, BankAccountViewModel bankAccountVm)
        {
            bankAccount.BankName = bankAccountVm.BankName;
            bankAccount.BankNumber = bankAccountVm.BankNumber;
        }

        public static void Update(this Complaint complaint, ComplaintViewModel complaintVm)
        {
            complaint.Modify(complaintVm.Message, complaintVm.ApplicationUserId
                , complaintVm.AdvertismentId, complaintVm.ComplainedId);
        }

        public static void Update(this ContactInformation contactInformation,
            ContactInformationViewModel contactInformationVm)
        {
            contactInformation.Modify(contactInformationVm.Contact
                , contactInformationVm.IconName
                , contactInformationVm.ContactTypeId);
        }


    }
}