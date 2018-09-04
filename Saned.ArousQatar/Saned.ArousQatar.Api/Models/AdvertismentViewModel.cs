using Saned.ArousQatar.Api.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using Saned.ArousQatar.Data.Core.Models;

namespace Saned.ArousQatar.Api.Models
{
    public class AdViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfViews { get; set; }
        public int NumberOfLikes { get; set; }
        public int CategoryId { get; set; }
        public int? Comments { get; set; }
        //public string CategoryName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsLiked { get; set; }
        public bool? IsFavorit { get; set; }
        public bool? IsReport { get; set; }
        public decimal? PaidEdPrice { get; set; }
        public decimal Cost { get; set; }
        public bool IsPaided { get; set; }
        public bool IsOwner { get; set; } = false;
        public bool IsActive { get; set; }
        public int? AdvertismentPriceId { get; set; }
        public bool IsExpired { get; set; } = true;

        public string FullName { get; set; }
        public string PhotoUrl { get; set; }

        //public List<AdvertismentImage> Images { get; set; }

        public List<AdImage> ImagesList { get; set; }

      

        public string JsonImages { get; set; }
       



    }

    public class AdvertismentViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserFirstName { get; set; }
        public string UserName { get; set; }

        public bool IsPaided { get; set; } = false;
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfViews { get; set; }
        public int NumberOfLikes { get; set; }
        public int? Comments { get; set; }
        public decimal? PaidEdPrice { get; set; }
        public decimal Cost { get; set; }
        public bool? IsArchieved { get; set; } = false;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }

        public string CategoryName { get; set; }
        public int AdvertisementPeriod { get; set; }
        public decimal AdvertisementPrice { get; set; }


        public int CategoryId { get; set; }

        public string ApplicationUserId { get; set; }

        public int? AdvertismentPriceId { get; set; }


        //public List<AdImagesViewModel> ImagesViewModels { get; set; }
        public string ImagesViewModels { get; set; }

        public List<AdImage> Images
        {
            get
            {
                if (!string.IsNullOrEmpty(ImagesViewModels))
                    return JsonConvert.DeserializeObject<List<AdImage>>(ImagesViewModels);
                else return null;
            }
        }
        public List<AdImage> ImagesList { get; set; }


        //public string ImageFilename { get; set; }

        //public string ImageBase64 { get; set; }

        public bool IsActive { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new AdvertismentViewModelValidator();
            var res = validator.Validate(this);
            return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }


    public class AdImagesViewModel
    {
        public string ImageFilename { get; set; }

        public string ImageBase64 { get; set; }
        public bool IsMainImage { get; set; } = false;




    }

    public class AdImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public  object Upload { get; set; }

        


     




    }

}