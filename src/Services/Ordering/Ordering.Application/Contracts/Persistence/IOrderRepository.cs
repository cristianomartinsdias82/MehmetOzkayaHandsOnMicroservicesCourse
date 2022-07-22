using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(
            string userName,
            CancellationToken cancellationToken = default);
    }
}
