using Saned.ArousQatar.Api.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Saned.ArousQatar.Api.Models
{
    public class ComplaintViewModel : IValidatableObject
    {
        //public int Id { get; set; }
        //public string Message { get; set; }
        //public bool? IsArchieved { get; set; } = false;

        //public string ApplicationUserId { get; set; }

        //public int? AdvertismentId { get; set; }

        //public string ComplainedId { get; set; }

        //public string CamplaintUser { get; set; }
        //public string ComplainedUser { get; set; }
        //public string AdvertisementName { get; set; }


        public int Id { get; set; }
        public string Message { get; set; }
        public bool? IsArchieved { get; set; } = false;

        public string ApplicationUserId { get; set; }


        public int? AdvertismentId { get; set; }


        public string ComplainedId { get; set; }






        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ComplaintViewModelValidator();
            var res = validator.Validate(this);
            return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}