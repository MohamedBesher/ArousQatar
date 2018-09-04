using Saned.ArousQatar.Api.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Saned.ArousQatar.Api.Models
{
    public class CategoryViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconName { get; set; }
        public string ImageUrl { get; set; }

        public string ImageBase64 { get; set; }
        public string ImageFilename { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new CategoryViewModelValidator();
            var res = validator.Validate(this);
            return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}