using System.Threading.Tasks;
using API.Application.Commands;
using API.Application.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Domain.Aggregates.AirportAggregate;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly ILogger<AirportsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AirportsController(
            ILogger<AirportsController> logger,
            IMediator mediator,
            IMapper mapper
        )
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        /**
        Get fight by id.
        Returns not found response if there's no airport for the given id.
        **/
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var airport = await _mediator.Send(new GetByIdCommand<Airport>(id));
            // Return not found if no airport found.
            if (airport == null)
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    $"No airport found for the id {id}"
                );
            return Ok(_mapper.Map<AirportViewModel>(airport));
        }

        /**
        Create airport.
        **/
        [HttpPost]
        public async Task<IActionResult> Store([FromBody] CreateAirportCommand command)
        {
            try
            {
                var airport = await _mediator.Send(command);
                // Return the get airport url with the created response.
                var location =
                    Url.Action(nameof(GetById), new { id = airport.Id }) ?? $"/{airport.Id}";
                return Created(location, _mapper.Map<AirportViewModel>(airport));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
