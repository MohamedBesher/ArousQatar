using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class ContactTypeViewModelValidator : AbstractValidator<ContactTypeViewModel>
    {
        public ContactTypeViewModelValidator()
        {
            RuleFor(c => c.Type)
                .NotEmpty()
                .Length(1, 50);
        }
    }
}