using Application.Dtos.Profile;
using FluentValidation;

namespace DecorteeSystem.Validators
{
    public class UpdateLanguageValidator : AbstractValidator<UpdateLanguageDto>
    {
        public UpdateLanguageValidator()
        {
            RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Language is required.")
                .MaximumLength(20).WithMessage("Language must not exceed 20 characters.");
        }
    }
}