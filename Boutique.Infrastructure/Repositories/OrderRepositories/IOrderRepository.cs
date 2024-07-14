using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.OrderRepositories {
    public interface IOrderRepository {
        Task AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid id);
    }
}
