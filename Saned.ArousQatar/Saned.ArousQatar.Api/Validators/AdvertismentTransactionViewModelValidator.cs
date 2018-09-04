using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class AdvertismentTransactionViewModelValidator : AbstractValidator<AdvertismentTransactionViewModel>
    {
        public AdvertismentTransactionViewModelValidator()
        {
            RuleFor(a => a.AdvertismentId).NotEmpty().WithMessage("رقم الأعلان مطلوب"); ;
            RuleFor(a => a.PaymentId).NotEmpty().WithMessage("رقم الفاتورة مطلوب");
        }
    }
}