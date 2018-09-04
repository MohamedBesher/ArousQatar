using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class AdvertismentPriceViewModelValidator : AbstractValidator<AdvertismentPriceViewModel>
    {
        public AdvertismentPriceViewModelValidator()
        {
            RuleFor(a => a.Period)
                .NotEmpty()
                .WithMessage("من فضلك ادخل المدة");

            RuleFor(a => a.Price)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("من فضلك ادخل السعر ويجب ان يكون اكبر من صفر");
        }
    }
}