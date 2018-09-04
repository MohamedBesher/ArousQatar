using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class AdvertismentImageViewModelValidator : AbstractValidator<AdvertisementImageViewModel>
    {
        public AdvertismentImageViewModelValidator()
        {
            RuleFor(a => a.AdvertismentId).NotEmpty();
            RuleFor(a => a.ImageUrl).NotEmpty().WithMessage("مسار الصوره مطلوب");
        }
    }
}