using Boutique.Application.Dtos.OrderDtos;
using Boutique.Application.Dtos.PaymentDtos;
using Boutique.Domain.Entities;
using Iyzipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.OrderServices {
    public interface IOrderService {
        Task<PaymentResponseDTO> CreateOrderAsync(string cartId,Dtos.PaymentDtos.PaymentCard paymentCard);
        Task<OrderResultDto> HandlePaymentCallbackAsync(string paymentToken);
    }
}
