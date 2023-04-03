using System.Collections.Generic;
using Domain.Aggregates.OrderAggregate;
using MediatR;

namespace API.Application.Commands
{
    /**
    Create order request takes email and flight rates <flight rate id, quantity> as params.
    **/
    public class CreateOrderCommand : IRequest<Order>
    {
        public string Email { get; private set; }

        public List<FlightRateRequest> FlightRates { get; private set; }

        public CreateOrderCommand(string email, List<FlightRateRequest> flightRates)
        {
            Email = email;
            FlightRates = flightRates;
        }
    }
}
