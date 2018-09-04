using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Saned.ArousQatar.Api.Validators;

namespace Saned.ArousQatar.Api.Models
{
    public class AdvertismentTransactionViewModel : IValidatableObject
    {
        
        public string PaymentId { get; set; }

        public int AdvertismentId { get; set; }

        public DateTime CreateDate { get; private set; }=DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new AdvertismentTransactionViewModelValidator();
            var res = validator.Validate(this);
            return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }


   
}