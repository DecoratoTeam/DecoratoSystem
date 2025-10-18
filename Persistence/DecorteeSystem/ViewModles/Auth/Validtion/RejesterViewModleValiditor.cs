using DecorteeSystem.Abastraction.Consts;
using FluentValidation;

namespace DecorteeSystem.ViewModles.Auth.Validtion
{
    public class RejesterViewModleValiditor : AbstractValidator<RejesterViewModle>
    {
        public RejesterViewModleValiditor()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Name).NotEmpty();

            RuleFor(x => x.Password).NotEmpty().Matches(RegexPatterns.Password).
                WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}