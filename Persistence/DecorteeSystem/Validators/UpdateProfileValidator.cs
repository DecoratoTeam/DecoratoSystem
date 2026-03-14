using Application.Dtos.Profile;
using FluentValidation;

namespace DecorteeSystem.Validators
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Bio must not exceed 500 characters.");
        }
    }
}