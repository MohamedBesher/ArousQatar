using Saned.ArousQatar.Api.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Saned.ArousQatar.Api.Models
{
    public class BankAccountViewModel : IValidatableObject
    {

        public int Id { get; set; }
        public string BankNumber { get; set; }
        public string BankName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new BankAccountViewModelValidator();
            var res = validator.Validate(this);
            return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}