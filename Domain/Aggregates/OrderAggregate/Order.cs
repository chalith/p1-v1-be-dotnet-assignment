using Domain.SeedWork;
using Domain.Common;
using Domain.Events;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Domain.Aggregates.OrderAggregate
{
    /**
    Order can have mutiple rates from multiple flights but in the same currency.
    Relationship between Order and FlightRate is one to many.
    **/
    public class Order : Entity, IAggregateRoot
    {
        public OrderStatus Status { get; private set; } = OrderStatus.Draft;

        public string Email { get; private set; }

        public Price Price { get; private set; }

        public int TotalQuantity { get; private set; }

        private List<OrderFlightRate> _orderFlightRates;
        public IReadOnlyCollection<OrderFlightRate> OrderFlightRates => _orderFlightRates;

        /**
        Constructor to instantiate new order.
        **/
        protected Order()
        {
            _orderFlightRates = new List<OrderFlightRate>();
            // Calculate the total order quantity.
            TotalQuantity = _orderFlightRates.Sum(ofr => ofr.Quantity);
        }

        /**
        Constructor to instantiate order without rates.
        **/
        public Order(string email)
            : this()
        {
            email = email.Trim();
            // If the email is not valid.
            if (new MailAddress(email).Address != email)
            {
                throw new ArgumentException("Provided email is invalid.");
            }

            Email = email;
        }

        /**
        Constructor to instantiate order with email and rates.
        **/
        public Order(string email, List<OrderFlightRate> orderFlightRates)
            : this()
        {
            email = email.Trim();
            // If the email is not valid.
            if (new MailAddress(email).Address != email)
            {
                throw new ArgumentException("Provided email is invalid.");
            }

            Email = email;
            _orderFlightRates = orderFlightRates;
            // Calculate the total order quantity.
            TotalQuantity = _orderFlightRates.Sum(ofr => ofr.Quantity);
        }

        /**
        Constructor to instantiate order with id, email and rates for update.
        **/
        public Order(Guid id, List<OrderFlightRate> orderFlightRates)
            : this()
        {
            Id = id;
            _orderFlightRates = orderFlightRates;
            // Calculate the total order quantity.
            TotalQuantity = _orderFlightRates.Sum(ofr => ofr.Quantity);
        }

        /**
        Set an order price.
        This will be the total of all the flight rate prices in the order.
        @param Price price New price object
        **/
        public void SetOrderPrice(Price price)
        {
            Price = price;
        }

        /**
        Add new flight rate to the order.
        @param Guild flightRateId Id of the new flight rate.
        @param int quantity Quantity needed from this flight rate.
        **/
        public void AddOrderFlightRate(Guid flightRateId, int quantity)
        {
            // If the order is in draft state, Do not allow update.
            if (Status != OrderStatus.Draft)
            {
                throw new ArgumentException("Order can only be updated in draft status");
            }

            // Add new flight rate to the order.
            var orderFlightRate = new OrderFlightRate(flightRateId, quantity);
            _orderFlightRates.Add(orderFlightRate);
            // Calculate the new order quantity.
            TotalQuantity += orderFlightRate.Quantity;
        }

        /**
        Remove all the flight rates.
        **/
        public void ClearFlightRates()
        {
            // If the order is in draft state, Do not allow update.
            if (Status != OrderStatus.Draft)
            {
                throw new ArgumentException("Order can only be updated in draft status");
            }

            // Clear the list.
            _orderFlightRates.Clear();
            // Clear the total quantity.
            TotalQuantity = 0;
        }

        /**
        Remove ordered flight rates.
        @param List<Guid> orderFlightRateIds List of orderFlightRate ids to remove
        **/
        public void RemoveFlightRates(List<Guid> orderFlightRateIds)
        {
            // If the order is in draft state, Do not allow update.
            if (Status != OrderStatus.Draft)
            {
                throw new ArgumentException("Order can only be updated in draft status");
            }

            // Remove the flight rates which are has ids in orderFlightRateIds
            _orderFlightRates.RemoveAll(o => orderFlightRateIds.Contains(o.Id));
            // Calculate the total order quantity.
            TotalQuantity = _orderFlightRates.Sum(ofr => ofr.Quantity);
        }

        /**
        Update the quantity of given flight rate id.
        @param Guid orderFlightRateId OrderFlightRateId to update.
        @param int quantity New quantity.
        **/
        public void UpdateQuantity(Guid orderFlightRateId, int quantity)
        {
            // If the order is in draft state, Do not allow update.
            if (Status != OrderStatus.Draft)
            {
                throw new ArgumentException("Order can only be updated in draft status");
            }

            var orderFlightRate = GetFlightRate(orderFlightRateId);
            var oldQuantity = orderFlightRate.Quantity;
            orderFlightRate.ChangeQuantity(quantity);

            TotalQuantity += (quantity - oldQuantity);
        }

        /**
        Get the added flight rate by Id.
        @param Guid orderFlightRateId FlightRateId to get.
        @returns OrderFlightRate Found OrderFlightRate object.
        **/
        private OrderFlightRate GetFlightRate(Guid orderFlightRateId)
        {
            var orderFlightRate = _orderFlightRates.SingleOrDefault(o => o.Id == orderFlightRateId);

            // Throw an exception if there's not such flight rate in the order.
            if (orderFlightRate == null)
            {
                throw new ArgumentException(
                    "This order does not contain a rate with the provided rateId"
                );
            }

            return orderFlightRate;
        }

        /**
        Change the order status to confirm.
        **/
        public void Confirm()
        {
            // If the order is in draft state, Do not allow update.
            if (Status != OrderStatus.Draft)
            {
                throw new ArgumentException("Order can only be updated in draft status");
            }

            Status = OrderStatus.Confirmed;

            // Send the domain event after order confirm.
            AddDomainEvent(new OrderConfirmedEvent(this));
        }
    }
}
