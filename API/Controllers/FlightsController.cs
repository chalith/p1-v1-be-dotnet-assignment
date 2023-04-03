using System.Threading.Tasks;
using API.Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly IMediator _mediator;

        public FlightsController(ILogger<FlightsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /**
        Search for available flight for a destination.
        Destination param could be an airport code or airport name.
        Returns not found respons if there's no available flight.
        **/
        [HttpGet]
        public async Task<IActionResult> GetAvailableFlights(string destination)
        {
            // Create new command and direct to the mediator.
            var flights = await _mediator.Send(new SearchFlightsCommand(destination));
            // Return not found if no flight found.
            if (flights.Count == 0)
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    $"No flight found to the destination {destination}"
                );
            return Ok(flights);
        }
    }
}
