using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Aggregates.OrderAggregate;
using Domain.SeedWork;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositores
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FlightsContext _context;

        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public OrderRepository(FlightsContext context)
        {
            _context = context;
        }

        /**
        Add new order to the data context.
        @param Order order Order to add.
        @returns Order Added order object.
        **/
        public async Task<Order> AddAsync(Order order)
        {
            Price totalPrice = null;
            if (order.OrderFlightRates.Count > 0)
            {
                // Loop through all the flight rates and calculate the price and check whether
                // they are in the same currency.
                foreach (var ofr in order.OrderFlightRates)
                {
                    var fr = _context.FlightRates.FirstOrDefault(o => o.Id == ofr.FlightRateId);
                    // If the rate does not exist, Throw an exception.
                    if (fr == null)
                    {
                        throw new ArgumentException(
                            $"Flght rate {ofr.FlightRateId} in the order is not available"
                        );
                    }
                    // Throw an exception if there's not available space.
                    else if (fr.Available < ofr.Quantity)
                    {
                        throw new ArgumentException("Requested quantity is not available");
                    }

                    // Total price for the rate is unit price muliplied by quantity ordered.
                    var price = fr.Price.Value * ofr.Quantity;

                    // Calculate the total price from the rate price and the quantity.
                    if (totalPrice == null)
                    {
                        totalPrice = new Price(price, fr.Price.Currency);
                    }
                    // Throw an exception if rate price currencies are different.
                    // Only one curreny can be used for an order.
                    else if (fr.Price.Currency != totalPrice.Currency)
                    {
                        throw new ArgumentException(
                            "Only flight rates from same currency are allowed"
                        );
                    }
                    else
                    {
                        // Add this rate price to existing price
                        totalPrice.AddValue(price);
                    }
                }
            }

            // Update the order price.
            order.SetOrderPrice(totalPrice);
            return (await _context.Orders.AddAsync(order)).Entity;
        }

        /**
        Update an order in the data context.
        @param Order order Order to update.
        @returns Order updated order object.
        **/
        public async Task<Order> UpdateAsync(Order order)
        {
            // Check for the existing order and throw an exception if not.
            var existingOrder = await _context.Orders
                .Include(o => o.OrderFlightRates)
                .ThenInclude(ofr => ofr.FlightRate)
                .FirstOrDefaultAsync(o => o.Id == order.Id);
            if (existingOrder == null)
            {
                throw new ArgumentException("No order for given id");
            }

            // Clear if there are no orders in updated order.
            // Otherwise update the existing orders.
            if (order.OrderFlightRates.Count > 0)
            {
                // Loop through all the flight rates in the given order and
                // update if rate already exist in the order. Oterwise add the new rate
                foreach (var ofr in order.OrderFlightRates)
                {
                    var existingOfr = existingOrder.OrderFlightRates.FirstOrDefault(
                        o => o.FlightRateId == ofr.FlightRateId
                    );
                    // If there's a existing order, update it. Otherwise add new order.
                    if (existingOfr != null)
                    {
                        // If the requested uantity does not exist throw an exception.
                        if (existingOfr.FlightRate.Available < ofr.Quantity)
                        {
                            throw new ArgumentException(
                                $"Only {existingOfr.FlightRate.Available} available"
                            );
                        }

                        // Update the order price and quantity.
                        existingOrder.Price.AddValue(
                            (ofr.Quantity - existingOfr.Quantity)
                                * existingOfr.FlightRate.Price.Value
                        );
                        existingOrder.UpdateQuantity(existingOfr.Id, ofr.Quantity);
                    }
                    else
                    {
                        // Throw an exception if there's not such flight rate.
                        var fr = _context.FlightRates.FirstOrDefault(o => o.Id == ofr.FlightRateId);
                        if (fr == null)
                        {
                            throw new ArgumentException("No flight rate for given id");
                        }
                        // Update the order price and add rate.
                        existingOrder.Price.AddValue(ofr.Quantity * fr.Price.Value);
                        existingOrder.AddOrderFlightRate(ofr.FlightRateId, ofr.Quantity);
                    }
                }
            }
            else
            {
                // Clear if update request has no rates.
                existingOrder.ClearFlightRates();
            }

            // Clear the flight rates which are not included in the updated order.
            var toClear = existingOrder.OrderFlightRates.Where(
                o => !order.OrderFlightRates.Any(ofr => ofr.FlightRateId == o.FlightRateId)
            );
            if (toClear.Count() > 0)
            {
                // Update the price and remove the orders.
                existingOrder.Price.AddValue(
                    toClear.Sum(o => (o.FlightRate.Price.Value * o.Quantity) * -1)
                );
                existingOrder.RemoveFlightRates(toClear.Select(o => o.Id).ToList());
            }

            return _context.Orders.Update(existingOrder).Entity;
        }

        /**
        Confirm a created order.
        @param Guid orderId Order id to confirm.
        @returns Order Updated order object.
        **/
        public async Task<Order> ConfirmAsync(Guid orderId)
        {
            // Get the existing order.
            var order = await _context.Orders
                .Include(o => o.OrderFlightRates)
                .ThenInclude(ofr => ofr.FlightRate)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            // Throw an exception if there's not such order.
            if (order == null)
            {
                throw new ArgumentException("There's no order for given id");
            }
            // Throw an exception if there are orders in which the quantity is not updated yet.
            else if (order.OrderFlightRates.Any(o => o.Quantity == 0))
            {
                throw new ArgumentException(
                    "There're flight rates which has 0 quantity in this order"
                );
            }

            // Change the order status.
            order.Confirm();

            // Update the new availability of existing FlightRates in the data context.
            // Reduce the ordered quantity from the availability.
            foreach (var r in order.OrderFlightRates)
            {
                // Throw an exception if the availability is not enough.
                if (r.Quantity > r.FlightRate.Available)
                {
                    throw new ArgumentException(
                        $"Flight {r.FlightRate.Name} has no {r.Quantity} available slots"
                    );
                }
                // Update the availability.
                else
                {
                    r.FlightRate.MutateAvailability(-1 * r.Quantity);
                    _context.FlightRates.Update(r.FlightRate);
                }
            }

            // Update the order and return.
            return _context.Orders.Update(order).Entity;
        }

        /**
        Get the order by id.
        @Guid orderId Id of the order.
        @returns Order The order object.
        **/
        public async Task<Order> GetAsync(Guid orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
