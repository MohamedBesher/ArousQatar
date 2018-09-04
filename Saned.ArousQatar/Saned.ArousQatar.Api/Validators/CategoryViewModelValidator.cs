using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
    {
        public CategoryViewModelValidator()
        {
            RuleFor(c => c.Name).NotEmpty()
               .Length(1, 100)
               .WithMessage("Category Name must be between 1 - 100 character");


            RuleFor(c => c.IconName).NotEmpty()
                .Length(1, 50)
                .WithMessage("اسم الأيقونه مطلوب");

        }
    }

    public class ContactUsMessageViewModelValidator : AbstractValidator<ContactUsMessageViewModel>
    {
        public ContactUsMessageViewModelValidator()
        {
           

        }
    }
}