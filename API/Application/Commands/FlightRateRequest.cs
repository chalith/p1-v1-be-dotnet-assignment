using System;

namespace API.Application.Commands
{
    /**
    Flight rate with flight rate id and quantity to send in an order.
    **/
    public class FlightRateRequest
    {
        public Guid FlightRateId { get; private set; }
        public int Quantity { get; private set; }

        public FlightRateRequest(Guid flightRateId, int quantity)
        {
            FlightRateId = flightRateId;
            Quantity = quantity;
        }
    }
}
