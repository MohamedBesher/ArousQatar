using FluentValidation;
using Saned.ArousQatar.Api.Models;

namespace Saned.ArousQatar.Api.Validators
{
    public class FavoriteViewModelValidator : AbstractValidator<FavoriteViewModel>
    {
        public FavoriteViewModelValidator()
        {
            RuleFor(f => f.AdvertismentId).NotEmpty();
            RuleFor(f => f.ApplicationUserId).NotEmpty();
        }
    }
}