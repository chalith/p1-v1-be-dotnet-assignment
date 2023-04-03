using System.Threading;
using System.Threading.Tasks;
using System;
using MediatR;
using Domain.Events;

namespace API.Application.DomainEventHandles
{
    public class OrderConfirmedDomainEventHandler : INotificationHandler<OrderConfirmedEvent>
    {
        public OrderConfirmedDomainEventHandler() { }

        /**
        Handle the event sent from order confirmation domain events.
        **/
        public Task Handle(
            OrderConfirmedEvent orderConfirmedDomainEvent,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine($"Order {orderConfirmedDomainEvent.Order.Id} confirmed");
            return Task.CompletedTask;
        }
    }
}
