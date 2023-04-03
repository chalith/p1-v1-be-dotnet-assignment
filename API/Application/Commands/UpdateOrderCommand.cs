using System.Collections.Generic;
using System;
using Domain.Aggregates.OrderAggregate;
using MediatR;

namespace API.Application.Commands
{
    /**
    Update order request takes flight rates to update and add <flight rate id, quantity> as params.
    Existing flight rates which are not included in this request will be deleted.
    **/
    public class UpdateOrderCommand : IRequest<Order>
    {
        public Guid Id { get; private set; }

        public List<FlightRateRequest> FlightRates { get; private set; }

        public UpdateOrderCommand(Guid id, List<FlightRateRequest> flightRates)
        {
            Id = id;
            FlightRates = flightRates;
        }
    }
}
