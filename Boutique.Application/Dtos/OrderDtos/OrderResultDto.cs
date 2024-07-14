using Boutique.Domain.Entities;
using Iyzipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Dtos.OrderDtos {
    public class OrderResultDto {
        public Order Order { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public CheckoutForm CheckoutForm { get; set; }
    }
}
