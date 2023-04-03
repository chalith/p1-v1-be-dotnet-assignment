using System.Threading;
using System.Threading.Tasks;
using Domain.Aggregates.AirportAggregate;
using MediatR;

namespace API.Application.Commands
{
    /**
    All the airport request handlers are implemented.
    **/
    public class AirportCommandHandler
        : IRequestHandler<GetByIdCommand<Airport>, Airport>,
            IRequestHandler<CreateAirportCommand, Airport>
    {
        private readonly IAirportRepository _airportRepository;

        public AirportCommandHandler(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }

        /**
        Handle get airport by id request.
        **/
        public async Task<Airport> Handle(
            GetByIdCommand<Airport> request,
            CancellationToken cancellationToken
        )
        {
            return await _airportRepository.GetAsync(request.Id);
        }

        /**
        Handle create airport request.
        **/
        public async Task<Airport> Handle(
            CreateAirportCommand request,
            CancellationToken cancellationToken
        )
        {
            // Add airport to the repository and save the context.
            var airport = _airportRepository.Add(new Airport(request.Code, request.Name));

            await _airportRepository.UnitOfWork.SaveEntitiesAsync();

            return airport;
        }
    }
}
