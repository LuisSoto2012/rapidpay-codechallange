using FluentValidation;
using RapidPay.Domain.Dto.Request;

namespace RapidPay.ServiceHost.Validators
{
    public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
    {
        public CreateCardRequestValidator()
        {
            RuleFor(x => x.Number)
                .NotNull().WithMessage("Number is required")
                .NotEmpty().WithMessage("Number is required");

            RuleFor(x => x.Balance)
                .GreaterThan(0).WithMessage("Balance should be greater than zero");
        }
    }
}