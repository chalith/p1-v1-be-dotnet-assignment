using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Domain.Aggregates.OrderAggregate;
using MediatR;

namespace API.Application.Commands
{
    /**
    All the order request handlers are implemented.
    **/
    public class OrderCommandHandler
        : IRequestHandler<GetByIdCommand<Order>, Order>,
            IRequestHandler<CreateOrderCommand, Order>,
            IRequestHandler<UpdateOrderCommand, Order>,
            IRequestHandler<ConfirmOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /**
        Handle get order by id request.
        **/
        public async Task<Order> Handle(
            GetByIdCommand<Order> request,
            CancellationToken cancellationToken
        )
        {
            return await _orderRepository.GetAsync(request.Id);
        }

        /**
        Handle create order request.
        **/
        public async Task<Order> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken
        )
        {
            // Cast incomming request to Domain models in order to pass to the repository.
            var flightRates = request.FlightRates
                .Select(r => new OrderFlightRate(r.FlightRateId, r.Quantity))
                .ToList();
            Order order = await _orderRepository.AddAsync(new Order(request.Email, flightRates));

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();

            return order;
        }

        /**
        Handle update order request.
        **/
        public async Task<Order> Handle(
            UpdateOrderCommand request,
            CancellationToken cancellationToken
        )
        {
            // Cast incomming request to Domain models in order to pass to the repository.
            var flightRates = request.FlightRates
                .Select(r => new OrderFlightRate(r.FlightRateId, r.Quantity))
                .ToList();
            Order order = await _orderRepository.UpdateAsync(new Order(request.Id, flightRates));

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();

            return order;
        }

        /**
        Handle confirm order request.
        **/
        public async Task<Order> Handle(
            ConfirmOrderCommand request,
            CancellationToken cancellationToken
        )
        {
            var order = await _orderRepository.ConfirmAsync(request.Id);

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();

            return order;
        }
    }
}
