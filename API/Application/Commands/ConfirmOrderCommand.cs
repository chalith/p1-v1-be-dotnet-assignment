using System;
using Domain.Aggregates.OrderAggregate;
using MediatR;

namespace API.Application.Commands
{
    /**
    Confirm order request takes order id as a param.
    **/
    public class ConfirmOrderCommand : IRequest<Order>
    {
        public Guid Id { get; private set; }

        public ConfirmOrderCommand(Guid id)
        {
            Id = id;
        }
    }
}
