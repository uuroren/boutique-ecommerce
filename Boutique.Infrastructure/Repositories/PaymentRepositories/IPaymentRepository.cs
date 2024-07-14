using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.PaymentRepositories {
    public interface IPaymentRepository {
        Task AddAsync(Payment payment);
        Task<Payment> GetByPaymentTokenAsync(string paymentToken);
    }
}
