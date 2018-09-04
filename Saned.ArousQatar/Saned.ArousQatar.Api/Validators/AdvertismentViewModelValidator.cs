using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class AdvertismentViewModelValidator : AbstractValidator<AdvertismentViewModel>
    {
        public AdvertismentViewModelValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .Length(1, 50)
                .WithMessage("ادخل اسم الاعلان");




                //RuleFor(x => x.ImagesViewModels)
                //.Must(x => x.Count > 0)
                //.WithMessage("على الاقل صورة واحدة .");
            //RuleFor(a => a.EndDate)
            //    .GreaterThan(a => a.StartDate)
            //    .WithMessage("تاريخ الانتهاء يجب ان يكون اكبر من تاريخ البداية");

            //RuleFor(a => a.StartDate)
            //    .LessThan(a => a.EndDate)
            //    .WithMessage("تاريخ البدء يجب ان يكون اصغر من تاريخ الانتهاء");

            RuleFor(a => a.IsPaided)
                .NotNull();

            RuleFor(a => a.CategoryId)
                .NotEmpty();

            RuleFor(a => a.PaidEdPrice)
                .NotNull();

            RuleFor(a => a.NumberOfLikes)
                .NotNull();

            RuleFor(a => a.NumberOfViews)
                .NotNull();

            //RuleFor(a => a.ImageUrl)
            //    .NotNull().WithMessage("ادخل صورةالاعلان");

        }
    }
}