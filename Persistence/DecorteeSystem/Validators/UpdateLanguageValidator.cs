using Application.Dtos.Profile;
using FluentValidation;

namespace DecorteeSystem.Validators
{
    public class UpdateLanguageValidator : AbstractValidator<UpdateLanguageDto>
    {
        public UpdateLanguageValidator()
        {
            RuleFor(x => x.Language)
                .NotEmpty()
                .Must(x => x is "en" or "ar")
                .WithMessage("Language must be either 'en' or 'ar'");
        }
    }
}
