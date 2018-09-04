using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class LikeViewModelValidator : AbstractValidator<LikeViewModel>
    {
        public LikeViewModelValidator()
        {
            RuleFor(l => l.AdvertismentId).NotEmpty();
            //RuleFor(l => l.ApplicationUserId).NotEmpty();
        }
    }
}