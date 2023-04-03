using API.ApiResponses;
using System.Collections.Generic;
using MediatR;

namespace API.Application.Commands
{
    /**
    Create order request takes destination as a param.
    **/
    public class SearchFlightsCommand : IRequest<List<FlightResponse>>
    {
        public string Destination { get; private set; }

        public SearchFlightsCommand(string destination)
        {
            Destination = destination;
        }
    }
}
