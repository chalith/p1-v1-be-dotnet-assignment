using API.Application.Commands;
using FluentValidation;

namespace API.Application.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        /**
        Validate email address and flight rate list.
        **/
        public UpdateOrderCommandValidator()
        {
            RuleFor(c => c.FlightRates).NotEmpty();
            RuleForEach(c => c.FlightRates).SetValidator(new FlightRateValidator());
        }
    }
}
