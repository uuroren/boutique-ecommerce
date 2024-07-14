using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.PaymentDtos {
    public class PaymentResponseDTO {
        public string Status { get; set; }
        public string PaymentPageUrl { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
