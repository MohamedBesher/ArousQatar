using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator()
        {
            RuleFor(c => c.Message)
                .NotEmpty()
                .WithMessage("من فضلك ادخل تعليق");
        }
    }
}