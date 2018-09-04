using Saned.ArousQatar.Api.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Saned.ArousQatar.Api.Models
{
    public class CommentViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public int AdvertismentId { get; set; }
        public string ApplicationUserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserFullName { get; set; }
        public int? CommentParentId { get; set; }

        public List<CommentViewModel> Replys { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new CommentViewModelValidator();
            var res = validator.Validate(this);
            return res.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}