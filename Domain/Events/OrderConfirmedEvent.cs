using Domain.Aggregates.OrderAggregate;
using MediatR;

namespace Domain.Events
{
    /**
    Notification event for order confirmation.
    **/
    public class OrderConfirmedEvent : INotification
    {
        public Order Order { get; private set; }

        public OrderConfirmedEvent(Order order)
        {
            Order = order;
        }
    }
}
