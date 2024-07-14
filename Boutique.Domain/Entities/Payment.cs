using Boutique.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Domain.Entities {
    public class Payment:BaseEntity {
        public string PaymentToken { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string CartId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public List<PaymentTransaction>? Transactions { get; set; }
    }

}
