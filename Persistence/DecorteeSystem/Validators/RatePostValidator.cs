using Application.Dtos.Profile;
using FluentValidation;

namespace DecorteeSystem.Validators
{
    public class RatePostValidator : AbstractValidator<RatePostDto>
    {
        public RatePostValidator()
        {
            RuleFor(x => x.Value)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
        }
    }
}