using FluentValidation;
using RapidPay.Domain.Dto.Request;

namespace RapidPay.Api.Validators
{
    public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
    {
        public CreateCardRequestValidator()
        {
            RuleFor(x => x.Number)
                .NotNull().WithMessage("Number is required")
                .NotEmpty().WithMessage("Number is required")
                .Matches("^[0-9]*$").WithMessage("Only numeric characters are allowed.")
                .Length(15).WithMessage("Number must have exactly 15 digits");

            RuleFor(x => x.Balance)
                .GreaterThan(0).WithMessage("Balance should be greater than zero");

            RuleFor(x => x.IdentificationNumber)
                .NotNull().WithMessage("IdentificationNumber is required")
                .NotEmpty().WithMessage("IdentificationNumber is required");
        }
    }
}