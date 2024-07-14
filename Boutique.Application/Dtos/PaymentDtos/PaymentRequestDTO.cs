using Iyzipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.PaymentDtos {
    public class PaymentRequestDTO {
        public string CartId { get; set; }
        public PaymentCard PaymentCard { get; set; }
    }
    public class PaymentCard {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }
        public string Cvc { get; set; }
        public int RegisterCard { get; set; }
    }
}
