using API.Application.Commands;
using FluentValidation;

namespace API.Application.Validators
{
    public class FlightRateValidator : AbstractValidator<FlightRateRequest>
    {
        /**
        Quantity should be greater than 0.
        **/
        public FlightRateValidator()
        {
            RuleFor(c => c.Quantity).GreaterThan(0);
        }
    }
}
