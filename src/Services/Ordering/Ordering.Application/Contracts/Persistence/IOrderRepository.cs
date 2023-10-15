using Ordering.Domain.Entities;

namespace Ordering.Application.Contracts.Persistence;
public interface IOrderRepository : IAsyncRepository<Order>
{
    public Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
}
