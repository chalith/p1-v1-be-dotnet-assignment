using Domain.SeedWork;
using Domain.Aggregates.FlightAggregate;
using System;

namespace Domain.Aggregates.OrderAggregate
{
    public class OrderFlightRate : Entity, IAggregateRoot
    {
        public Guid FlightRateId { get; private set; }

        public FlightRate FlightRate { get; private set; }

        public int Quantity { get; private set; }

        public OrderFlightRate() { }

        /**
        Constructor to instantiate order flight rate with rate id and quantity.
        **/
        public OrderFlightRate(Guid flightRateId, int quantity)
            : this()
        {
            FlightRateId = flightRateId;
            Quantity = quantity;
        }

        /**
        Constructor to instantiate order flight rate with rate object and quantity.
        **/
        public OrderFlightRate(FlightRate flightRate, int quantity)
            : this()
        {
            FlightRateId = flightRate.Id;
            FlightRate = flightRate;
            Quantity = quantity;
        }

        /**
        Change the quantity of the ordered flight rate.
        @param quantity New quantity of the flight rate.
        **/
        public void ChangeQuantity(int quantity)
        {
            Quantity = quantity;
        }
    }
}
