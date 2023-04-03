using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Domain.Aggregates.FlightAggregate;
using API.ApiResponses;
using System.Collections.Generic;
using MediatR;

namespace API.Application.Commands
{
    /**
    All the flight request handlers are implemented.
    **/
    public class FlightCommandHandler
        : IRequestHandler<GetByIdCommand<Flight>, Flight>,
            IRequestHandler<SearchFlightsCommand, List<FlightResponse>>
    {
        private readonly IFlightRepository _flightRepository;

        public FlightCommandHandler(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        /**
        Handle get flight by id request.
        **/
        public async Task<Flight> Handle(
            GetByIdCommand<Flight> request,
            CancellationToken cancellationToken
        )
        {
            // Get the flight from repository with given id.
            return await _flightRepository.GetAsync(request.Id);
        }

        /**
        Handle search flight request.
        **/
        public async Task<List<FlightResponse>> Handle(
            SearchFlightsCommand request,
            CancellationToken cancellationToken
        )
        {
            // Search for available flights.
            var flights = await _flightRepository.GetAvailableAsync(request.Destination);

            // Cast flight models to response objects.
            return flights
                .Select(
                    f =>
                        new FlightResponse(
                            f.OriginAirport.Code,
                            f.DestinationAirport.Code,
                            f.Departure,
                            f.Arrival,
                            f.Rates.MinBy(r => r.Price.Value).Price.Value
                        )
                )
                .ToList();
        }
    }
}
