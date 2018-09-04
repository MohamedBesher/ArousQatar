using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class ComplaintViewModelValidator : AbstractValidator<ComplaintViewModel>
    {
        public ComplaintViewModelValidator()
        {
            RuleFor(c => c.Message)
                .NotEmpty()
                .WithMessage("من فضلك ادخل نص الشكوى");


        }
    }
}