using API.Application.Commands;
using FluentValidation;

namespace API.Application.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        /**
        Validate email address and flight rate list.
        **/
        public CreateOrderCommandValidator()
        {
            RuleFor(c => c.Email).NotEmpty().EmailAddress();
            RuleFor(c => c.FlightRates).NotEmpty();
            RuleForEach(c => c.FlightRates).SetValidator(new FlightRateValidator());
        }
    }
}
