using Application.Dtos.Profile;
using FluentValidation;

namespace DecorteeSystem.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
        }
    }
}
