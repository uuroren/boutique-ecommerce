using Boutique.Domain.Entities;
using Boutique.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.PaymentRepositories {
    public class PaymentRepository:IPaymentRepository {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task AddAsync(Payment payment) {
            payment.Id = Guid.NewGuid();
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment> GetByPaymentTokenAsync(string paymentToken) {
            return await _context.Payments.AsNoTracking().Include(p => p.Transactions).FirstOrDefaultAsync(p => p.PaymentToken == paymentToken);
        }
    }

}
