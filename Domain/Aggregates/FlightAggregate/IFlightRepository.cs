using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Domain.Aggregates.FlightAggregate
{
    public interface IFlightRepository
    {
        Task<Flight> AddAsync(Flight flight);

        void Update(Flight flight);

        Task<Flight> GetAsync(Guid flightId);

        Task<List<Flight>> GetAvailableAsync(string destination);
    }
}