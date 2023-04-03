using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Events;
using Domain.SeedWork;
using Domain.Aggregates.AirportAggregate;

namespace Domain.Aggregates.FlightAggregate
{
    public class Flight : Entity, IAggregateRoot
    {
        public Guid OriginAirportId { get; private set; }

        public Airport OriginAirport { get; private set; }

        public Guid DestinationAirportId { get; private set; }

        public Airport DestinationAirport { get; private set; }

        public DateTimeOffset Departure { get; private set; }

        public DateTimeOffset Arrival { get; private set; }

        private List<FlightRate> _rates;
        public IReadOnlyCollection<FlightRate> Rates => _rates;

        protected Flight()
        {
            _rates = new List<FlightRate>();
        }

        public Flight(
            DateTimeOffset departure,
            DateTimeOffset arrival,
            Guid originAirportId,
            Guid destinationAirportId
        )
            : this()
        {
            OriginAirportId = originAirportId;
            DestinationAirportId = destinationAirportId;
            Departure = departure;
            Arrival = arrival;
        }

        /**
        Add new flight rate to the flight.
        @param string name Name of the rate.
        @param Price Rate price.
        @param in numAvailable Number of available slots.
        **/
        public void AddRate(string name, Price price, int numAvailable)
        {
            var rate = new FlightRate(name, price, numAvailable);
            _rates.Add(rate);
        }

        /**
        Update price of added flight rate.
        @param Guid rateId Id of the flight rate.
        @param Price price New price.
        **/
        public void UpdateRatePrice(Guid rateId, Price price)
        {
            var rate = GetRate(rateId);

            // Update the price.
            rate.ChangePrice(price);

            AddDomainEvent(new FlightRatePriceChangedEvent(this, rate));
        }

        /**
        Update the flight rate availability.
        @param Guid rateId Id of the flight rate.
        @param int mutation new availabiity to add.
        **/
        public void MutateRateAvailability(Guid rateId, int mutation)
        {
            var rate = GetRate(rateId);

            rate.MutateAvailability(mutation);

            AddDomainEvent(new FlightRateAvailabilityChangedEvent(this, rate, mutation));
        }

        /**
        Get a flight rate by id.
        @param Guid rateId Rate id to get.
        **/
        private FlightRate GetRate(Guid rateId)
        {
            var rate = _rates.SingleOrDefault(o => o.Id == rateId);

            // Throw an exception if not exist.
            if (rate == null)
            {
                throw new ArgumentException(
                    "This flight does not contain a rate with the provided rateId"
                );
            }

            return rate;
        }
    }
}
