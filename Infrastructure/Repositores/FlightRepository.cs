using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Aggregates.FlightAggregate;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Infrastructure.Repositores
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightsContext _context;

        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public FlightRepository(FlightsContext context)
        {
            _context = context;
        }

        /**
        Add new flight to the data context.
        @param Flight flight Flight to add.
        @returns Flight Added flight object.
        **/
        public async Task<Flight> AddAsync(Flight flight)
        {
            return (await _context.Flights.AddAsync(flight)).Entity;
        }

        /**
        Update added flight in the data context.
        @param Flight flight Flight to update.
        @returns Flight updated flight object.
        **/
        public void Update(Flight flight)
        {
            _context.Flights.Update(flight);
        }

        /**
        Get flight by id from the data context.
        @param Guid flightId Id of the flight.
        @returns Flight The flight object.
        **/
        public async Task<Flight> GetAsync(Guid flightId)
        {
            return await _context.Flights.FirstOrDefaultAsync(o => o.Id == flightId);
        }

        /**
        Available flights by the destination from the data context.
        @param string destination Destination to search for.
        @returns List<Flight> Lis of available flight objects.
        **/
        public async Task<List<Flight>> GetAvailableAsync(string destination)
        {
            // Filter flights by airport name and code.
            return await _context.Flights
                .Include(f => f.DestinationAirport)
                .Include(f => f.OriginAirport)
                .Include(f => f.Rates)
                .Where(
                    f =>
                        (
                            f.DestinationAirport.Name == destination
                            || f.DestinationAirport.Code == destination
                        ) && f.Rates.Where(r => r.Available > 0).Any()
                )
                .ToListAsync();
        }
    }
}
