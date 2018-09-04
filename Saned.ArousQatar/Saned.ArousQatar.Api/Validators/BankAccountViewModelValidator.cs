using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class BankAccountViewModelValidator : AbstractValidator<BankAccountViewModel>
    {
        public BankAccountViewModelValidator()
        {
            RuleFor(b => b.BankName)
                .NotEmpty()
                .Length(1, 50)
                .WithMessage("من فضلك ادخل اسم البنك");

            RuleFor(b => b.BankNumber)
                .NotEmpty()
                .Length(1, 100)
                .CreditCard()                   //check here real bank account to see if it work
                .WithMessage("من فضلك ادخل رقم الحساب");
        }
    }
}