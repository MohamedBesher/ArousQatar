using Saned.ArousQatar.Api.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Saned.ArousQatar.Api.Models
{
    public class ContactUsMessageViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsArchieved { get; set; } = false;
       
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ContactUsMessageViewModelValidator();
            var res = validator.Validate(this);
            return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}