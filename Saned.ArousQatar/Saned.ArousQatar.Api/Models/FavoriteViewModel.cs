using Saned.ArousQatar.Api.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Saned.ArousQatar.Api.Models
{
    public class FavoriteViewModel //: IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfViews { get; set; }
        public int NumberOfLikes { get; set; }
        public int Comments { get; set; }
        public string ImageUrl { get; set; }
        public int AdvertismentId { get; set; }
        public string ApplicationUserId { get; set; }
        public decimal Cost { get; set; }
        public int OverAllCount { get; set; }

        public bool IsExpired { get; set; } = true;
        public bool? IsPaided { get; set; } = false;
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{

        //    var validator = new FavoriteViewModelValidator();
        //    var res = validator.Validate(this);
        //    return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        //}
    }
}