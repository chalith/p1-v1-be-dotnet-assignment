using System;
using System.Threading.Tasks;
using Domain.Aggregates.AirportAggregate;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositores
{
    public class AirportRepository : IAirportRepository
    {
        private readonly FlightsContext _context;

        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public AirportRepository(FlightsContext context)
        {
            _context = context;
        }

        /**
        Add airport.
        @param Airport airport instance.
        @returns Airport object.
        **/
        public Airport Add(Airport airport)
        {
            return _context.Airports.Add(airport).Entity;
        }

        /**
        Update airport.
        @param Airport airport instance to update.
        **/
        public void Update(Airport airport)
        {
            _context.Airports.Update(airport);
        }

        /**
        Get an airort by id.
        @param Guid airportId Id of the airport.
        @retuns Airport instance.
        **/
        public async Task<Airport> GetAsync(Guid airportId)
        {
            return await _context.Airports.FirstOrDefaultAsync(o => o.Id == airportId);
        }
    }
}
