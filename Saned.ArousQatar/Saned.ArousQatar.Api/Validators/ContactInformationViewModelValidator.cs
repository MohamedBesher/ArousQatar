using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class ContactInformationViewModelValidator : AbstractValidator<ContactInformationViewModel>
    {
        public ContactInformationViewModelValidator()
        {
            RuleFor(c => c.Contact)
                .NotEmpty()
                .WithMessage("من فضلك ادخل طريقة الاتصال");

            RuleFor(c => c.ContactTypeId).NotEmpty();

            RuleFor(c => c.IconName).NotEmpty().Length(1, 70).
                WithMessage("من فضلك ادخل ايقونة");
        }
    }
}