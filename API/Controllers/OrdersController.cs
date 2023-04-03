using System.Threading.Tasks;
using System;
using API.Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Application.ViewModels;
using Domain.Aggregates.OrderAggregate;
using Microsoft.AspNetCore.Http;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    public OrdersController(ILogger<OrdersController> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    /**
    Get order by id.
    Returns not found respons if there's no available flight.
    **/
    [HttpGet]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _mediator.Send(new GetByIdCommand<Order>(id));
        // Return not found if no order found.
        if (order == null)
            return StatusCode(StatusCodes.Status404NotFound, $"No order found for the id {id}");
        return Ok(_mapper.Map<OrderViewModel>(order));
    }

    /**
    Create new order.
    **/
    [HttpPost]
    public async Task<IActionResult> Store([FromBody] CreateOrderCommand command)
    {
        try
        {
            var order = await _mediator.Send(command);
            // Return the get order url with the created response.
            var location = Url.Action(nameof(GetById), new { id = order.Id }) ?? $"/{order.Id}";
            return Created(location, _mapper.Map<OrderViewModel>(order));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /**
    Update a created order.
    Update ordered flight rates and quantities.
    **/
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateOrderCommand command)
    {
        try
        {
            var order = await _mediator.Send(command);
            return Ok(_mapper.Map<OrderViewModel>(order));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /**
    Confirm a created order.
    **/
    [HttpPut]
    [Route("Confirm")]
    public async Task<IActionResult> ConfirmOrder(Guid id)
    {
        try
        {
            var order = await _mediator.Send(new ConfirmOrderCommand(id));
            return Ok(_mapper.Map<OrderViewModel>(order));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
