using Boutique.Domain.Entities;
using Boutique.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.OrderRepositories {
    public class OrderRepository:IOrderRepository {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task AddAsync(Order order) {
            order.Id = Guid.NewGuid();
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id) {
            return await _context.Orders.AsNoTracking().Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        }
    }

}
