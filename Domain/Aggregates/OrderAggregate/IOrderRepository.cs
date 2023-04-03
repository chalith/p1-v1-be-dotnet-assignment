using System;
using System.Threading.Tasks;
using Domain.SeedWork;

namespace Domain.Aggregates.OrderAggregate
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> AddAsync(Order order);

        Task<Order> UpdateAsync(Order order);

        Task<Order> ConfirmAsync(Guid orderId);

        Task<Order> GetAsync(Guid orderId);
    }
}
