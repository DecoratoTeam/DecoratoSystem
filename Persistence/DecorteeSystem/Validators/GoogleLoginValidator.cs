using DecorteeSystem.ViewModles.Auth;
using FluentValidation;

namespace DecorteeSystem.Validators
{
    public class GoogleLoginValidator : AbstractValidator<GoogleLoginViewModle>
    {
        public GoogleLoginValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("Google ID Token is required.");
        }
    }
}